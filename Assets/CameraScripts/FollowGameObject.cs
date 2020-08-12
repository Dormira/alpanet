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

    void Start()
    {
        //Default the camera to the first alpaca in the list
        //Concept: Maybe have there be a default camera mode where we orbit around the middle of the map?
        //Concept: An alpaca favoriting system, follow the first favorited alpaca? Requires save system, not yet implemented
        GameObject firstAlpaca = GameObject.FindGameObjectsWithTag("Alpaca")[0];
        setTarget(firstAlpaca);
    } 

    public void setTarget(GameObject alpaca)
    {
        target = alpaca.transform.GetChild(1).gameObject;

        UnityEngine.Debug.Log(target.transform.position);
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
                //Smoothdamp doesn't like being passed time as a variable, only as a reference
                transform.position = Vector3.SmoothDamp(this.transform.position,
                    target.transform.position, ref velocity, 0.3f);
            }
            else
            {
                velocity = new Vector3(0f, 0f, 0f);//This fixes camera jerki-ness from having leftover velocity
            }
            transform.LookAt(target.transform);
            birdseyePos = Vector3.zero;
        }
        else//If we are clicking and dragging I want the camera to exist some distance directly above the alpaca and stay there
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