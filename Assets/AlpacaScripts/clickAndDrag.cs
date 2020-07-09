using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System;
using UnityEngine;
//TODO: When you drag an object too fast it can clip through the ground
//Using moveto rather than magically teleporting the cube should do it
//That might be more expensive, computationally, so probably I just want to give gameobjects a location floor of <terrain z location at xy>


public class clickAndDrag : MonoBehaviour
{
    Vector3 oldMousePosition;
    Vector3 oldHandPosition;
    Vector3 offset;


    AlpacaVariables rootvars;

    void Start()
    {
        rootvars = transform.root.GetComponent<AlpacaVariables>();
    }

    void OnMouseUp()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rootvars.isClickingAndDragging = false;
    }

    void OnMouseDown()
    {
        UnityEngine.Debug.Log(transform.root);
        rootvars.isClickingAndDragging = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

        oldMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The original place that I clicked in screen points
    }

    /*
    void OnMouseDrag()
    {
        Vector3 newMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The current place that my mouse is in screen points
        Vector3 objScreenLocation = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 translatedObjScreenLocation = objScreenLocation + (newMousePosition - oldMousePosition);

        transform.position = Camera.main.ScreenToWorldPoint(translatedObjScreenLocation);
        //This code makes us not go below the ground
        Vector3 curpos = transform.position;
        curpos.y = Terrain.activeTerrain.SampleHeight(curpos)+5;
        if (transform.position.y < curpos.y)
        {
            transform.position = curpos;
        }

        oldMousePosition = newMousePosition;
    }
    */
    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//The ray of my mouse cursor
                                                                    //       Vector3 a = new Vector3(transform.position.x, )
        float groundDistance;
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 9999, 1 << LayerMask.NameToLayer("Terrain"));
        //  Vector3 newPoint = ray.GetPoint(Math.Min(40, hit.distance));
        Vector3 newPoint = ray.GetPoint(hit.distance-5);
        transform.position = newPoint;
    }
}
