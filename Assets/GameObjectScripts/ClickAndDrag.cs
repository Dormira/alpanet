using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    AlpacaVariables rootvars;

    void Start()
    {
        rootvars = transform.root.GetComponent<AlpacaVariables>();
    }

    void OnMouseUp()
    {
        //Unlock the rigidbody constraints and set the click and drag flag
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rootvars.isClickingAndDragging = false;
    }

    void OnMouseDown()
    {
        //Set the object's click and drag flag
        rootvars.isClickingAndDragging = true;
        //Constrain the object's position and zero out its velocities
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        //Set the primary camera to follow the target
        int targetIndex = SpawnManager.alpacaIndex.IndexOf(transform.root.name);
        Camera.main.gameObject.GetComponent<FollowGameObject>().setTarget(targetIndex);
    }

    void OnMouseDrag()
    {
        //Get the location on the terrain that we clicked and move the object to five units above that point

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        //Must ensure that the object we're using as ground is on the terrain layer
        Physics.Raycast(ray, out hit, 9999, 1 << LayerMask.NameToLayer("Terrain"));
        Vector3 newPoint = ray.GetPoint(hit.distance-1);
        transform.position = newPoint;
    }
}
