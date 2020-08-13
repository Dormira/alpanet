using System.Security.Cryptography;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    private Vector3 velocity = new Vector3(0f, 0f, 0f); 
    Vector3 birdseyePos;
    Vector3 lookpoint;

    //So currently we refer to alpacas specifically because those are the only objects that exist in our world for now
    //Eventually I'll introduce different objects at which point I'll shift away from the alpaca specific terminology
    int currentAlpacaIndex = 0;
    GameObject targetAlpacaMesh;
    AlpacaVariables alpvar;

    public void setTarget(int alpacaIndex)
    {
        //This should take an index number to SpawnManager
        currentAlpacaIndex = alpacaIndex;
        GameObject target = GameObject.Find(SpawnManager.alpacaIndex[alpacaIndex]);
        targetAlpacaMesh = target.transform.Find("AlpacaMesh").gameObject;

        alpvar = target.GetComponent<AlpacaVariables>();
    }

    void Update()
    {
        //If there's no alpaca then just have the camera sit pointing at nothing. It's fine.
        if(targetAlpacaMesh == null){
            return;
        }

        if (Input.GetKeyDown("left"))
        {
            //Decrement and wrap if necessary
            currentAlpacaIndex--;
            if (currentAlpacaIndex < 0)
            {
                currentAlpacaIndex = SpawnManager.alpacaIndex.Count - 1;
            }
            setTarget(currentAlpacaIndex);
        }
        else if (Input.GetKeyDown("right"))
        {
            //Increment and wrap if necessary
            currentAlpacaIndex++;
            if (currentAlpacaIndex > SpawnManager.alpacaIndex.Count - 1)
            {
                currentAlpacaIndex = 0;
            }
            setTarget(currentAlpacaIndex);
        }

        if (!alpvar.isClickingAndDragging)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetAlpacaMesh.transform.position);

            if (distanceToTarget > 20f)
            {
                //Smoothdamp doesn't like being passed velocity as a variable, only as a reference
                transform.position = Vector3.SmoothDamp(this.transform.position,
                    targetAlpacaMesh.transform.position, ref velocity, 0.3f);
            }
            else if (distanceToTarget < 5f)
            {
                Vector3 targetPosition = this.transform.position;
                targetPosition.y = targetPosition.y + 5;
                transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, 0.3f);
            }
            else
            {
                velocity = new Vector3(0f, 0f, 0f);//This fixes camera jerki-ness from having leftover velocity
            }
            transform.LookAt(targetAlpacaMesh.transform);
            birdseyePos = Vector3.zero;
        }
        else//If we are clicking and dragging I want the camera to exist some distance directly above the alpaca and stay there
        {
            if (birdseyePos == Vector3.zero)
            {
                birdseyePos = targetAlpacaMesh.transform.position;
                lookpoint = birdseyePos;
                birdseyePos.y = birdseyePos.y + 40;
            }
            if (Vector3.Distance(this.transform.position, birdseyePos) > 0.5f)
            {
                transform.position = Vector3.SmoothDamp(this.transform.position,
        birdseyePos, ref velocity, 0.5f);
                transform.LookAt(lookpoint);
            }
        }
    }
}