using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    private Vector3 velocity = new Vector3(0f, 0f, 0f);
    Vector3 birdseyePos;
    Vector3 lookpoint;
 
    GameObject target;
    AlpacaVariables alpvar;
    // Start is called before the first frame update
    void Start()
    {
        GameObject firstAlpaca = GameObject.FindGameObjectsWithTag("Alpaca")[0];
        setTarget(firstAlpaca);
        //TODO: disentangle this from specific object
        //target = GameObject.Find("AlpacaPrime").transform.GetChild(1).gameObject;
    //    target = firstAlpaca.transform.GetChild(1).gameObject;
    //    alpvar = firstAlpaca.GetComponent<AlpacaVariables>();
    } 

    public void setTarget(GameObject alpaca)
    {
        target = alpaca.transform.GetChild(1).gameObject;
        alpvar = alpaca.GetComponent<AlpacaVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!alpvar.isClickingAndDragging)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
 
            if (distanceToTarget > 20f)
            {
                //Smoothdamp doesn't like being passed time as a variable, not sure why
                transform.position = Vector3.SmoothDamp(this.transform.position,
                    target.transform.position, ref velocity, 0.3f);
            }
            transform.LookAt(target.transform);
            birdseyePos = Vector3.zero;
        }
        else//If we are clicking and dragging I want the camera to exist 20m directly above the alpaca and stay there
        {
            if(birdseyePos == Vector3.zero)
            {
                birdseyePos = target.transform.position;
                lookpoint = birdseyePos;
                birdseyePos.y = birdseyePos.y + 40;
            }
            if(Vector3.Distance(this.transform.position,birdseyePos) > 0.5f)
            {
                transform.position = Vector3.SmoothDamp(this.transform.position,
        birdseyePos, ref velocity, 0.5f);
                transform.LookAt(lookpoint);
            }




        }
        
        
        

    }
}