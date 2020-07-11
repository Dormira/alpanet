using UnityEngine;


public class clickAndDrag : MonoBehaviour
{
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
        rootvars.isClickingAndDragging = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

        GameObject.Find("MainCamera").GetComponent<FollowGameObject>().setTarget(transform.root.gameObject);
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

        RaycastHit hit;
        Physics.Raycast(ray, out hit, 9999, 1 << LayerMask.NameToLayer("Terrain"));
        Vector3 newPoint = ray.GetPoint(hit.distance-5);
        transform.position = newPoint;
    }
}
