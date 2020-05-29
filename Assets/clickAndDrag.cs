using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
//TODO: When you drag an object too fast it can clip through the ground
//Using moveto rather than magically teleporting the cube should do it
//That might be more expensive, computationally, so probably I just want to give gameobjects a location floor of 0
public class clickAndDrag : MonoBehaviour
{
    Vector3 screenPoint;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
    }

    void OnMouseDown()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The original place that I clicked in screen points
      //  offset = this.transform.position - Camera.main.ScreenToWorldPoint(screenPoint);//The difference between the object and where I clicked in world points
    }

    void OnMouseDrag()
    {
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        //This is clearly wrong.
        //What we want is to get the object's current position
        Vector3 curWorldPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The current place that my mouse is in screen points
        Vector3 objScreenLocation = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 translatedObjScreenLocation = objScreenLocation + (curWorldPoint - screenPoint);
        Vector3 newObjWorldLocation = Camera.main.ScreenToWorldPoint(translatedObjScreenLocation);

        this.transform.position = newObjWorldLocation;
        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);



    }
}
