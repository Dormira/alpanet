using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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

    //Offset is the difference between where I clicked and where the object is in world points
    //Movement should act like I clicked the object's location
    //We should take the difference between where we clicked and dragged in world point units
    //Called when you click?

    /*
    void OnMouseDown()
    {
        screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));//The original place that I clicked in world points
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));//The difference between the object and where I clicked in world points
        
    }

    void OnMouseDrag()
    {
        //This is clearly wrong.
        //What we want is to get the object's current position
        Vector3 curWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));//The current place that my mouse is in world points
        Vector3 diff = curWorldPoint - screenPoint;//The difference in how I moved my mouse in world points and where my mouse started in world points
        
      //  Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        UnityEngine.Debug.Log("OFFSET IS " + offset);
        UnityEngine.Debug.Log("DIFF IS " + diff);
        UnityEngine.Debug.Log("MOVING OBJECT FROM "+transform.position+" TO "+(transform.position+diff+offset));
     //   this.transform.position += offset+Camera.main.ScreenToWorldPoint(diff);
    }

    */

    void OnMouseUp()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
    }

    void OnMouseDown()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The original place that I clicked in screen points
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(screenPoint);//The difference between the object and where I clicked in world points
    }

    void OnMouseDrag()
    {
        //This is clearly wrong.
        //What we want is to get the object's current position
        Vector3 curWorldPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//The current place that my mouse is in screen points
        Vector3 diff = curWorldPoint - screenPoint;//The difference in where I moved my mouse and where my mouse started, in screen points

        //  Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        if (diff != new Vector3(0, 0, 0))
        {
            UnityEngine.Debug.Log("OFFSET IS " + offset);
            UnityEngine.Debug.Log("DIFF IS " + diff);
            UnityEngine.Debug.Log("MOVING OBJECT FROM " + this.transform.position + " TO " + (this.transform.position + diff));
            screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            offset = this.transform.position - Camera.main.ScreenToWorldPoint(screenPoint);
            this.transform.position += diff;
        }

    }
}
