using UnityEngine;

public class Gravity : MonoBehaviour
{
    float pullRadius = 1000;
    float pullForce = 300f;

    void Start()
    {
        //If we're freestyling gravity, disable Unity's
        Physics.gravity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius))
        {
            if (collider.name.Contains("Leg")){//TODO: figure out a more elegant way to only apply gravity to feet/keep the alpacas heads up
                Vector3 forceDirection = transform.position - collider.transform.position;

                collider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }
}
