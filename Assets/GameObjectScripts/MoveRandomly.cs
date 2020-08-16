using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    private float startTime;
    private float maxForce = 150f;//Max force should be alpaca dependent in the future

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        if (Time.time - startTime > 5)
        {
            startTime = Time.time;
            //Just push them in a direction 
            rb.velocity = new Vector3(UnityEngine.Random.Range(-maxForce, maxForce),
                            UnityEngine.Random.Range(-maxForce, maxForce),
                            UnityEngine.Random.Range(-maxForce, maxForce));
            UnityEngine.Debug.Log("APPLYING FORCE "+rb.velocity);
        }

    }

}
