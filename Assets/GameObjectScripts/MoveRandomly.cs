using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    private float startTime;

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
            rb.velocity = new Vector3(UnityEngine.Random.Range(-75f, 75f),
                            UnityEngine.Random.Range(0, 75f),
                            UnityEngine.Random.Range(-75f, 75f));
        }

    }

}
