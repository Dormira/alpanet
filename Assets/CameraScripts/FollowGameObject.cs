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

    public void setTarget(GameObject alpacaObject)
    {
        // setTarget is not guaranteed to be called by a function that will modify the currentAlpacaIndex,
        // and so currently swapping target alpacas via the GUI or by clicking and dragging or by spawning a new alpaca
        // may cause the first left/right after that event to fail
        targetAlpacaMesh = alpacaObject.transform.Find("AlpacaMesh").gameObject;

        alpvar = alpacaObject.GetComponent<AlpacaVariables>();
    }

    void Update()
    {
        //TODO: Theoretically, we could manage a counter in the spawner, so that we know the
        //number of alpacas without having to check each frame, but this shouldn't cause any
        //real performance issues for now.

        //This might actually be causing performance issues


        //If there's no alpaca then just have the camera sit pointing 
        if(targetAlpacaMesh == null){
            return;
        }

        if (Input.GetKeyDown("left"))
        {
            GameObject[] alpacaObjects = GameObject.FindGameObjectsWithTag("Alpaca");
            //Decrement and wrap if necessary
            currentAlpacaIndex--;
            if (currentAlpacaIndex < 0)
            {
                currentAlpacaIndex = alpacaObjects.Length - 1;
            }
            UnityEngine.Debug.Log(currentAlpacaIndex);
            setTarget(alpacaObjects[currentAlpacaIndex]);
        }
        else if (Input.GetKeyDown("right"))
        {
            GameObject[] alpacaObjects = GameObject.FindGameObjectsWithTag("Alpaca");
            //Increment and wrap if necessary
            currentAlpacaIndex++;
            if (currentAlpacaIndex > alpacaObjects.Length - 1)
            {
                currentAlpacaIndex = 0;
            }
            UnityEngine.Debug.Log(currentAlpacaIndex);
            setTarget(alpacaObjects[currentAlpacaIndex]);
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