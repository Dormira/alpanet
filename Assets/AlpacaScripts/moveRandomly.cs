using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRandomly : MonoBehaviour
{
    private float startTime;

    public Rigidbody rb;
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if(Time.time - startTime > 3)
        {
            startTime = Time.time;
            rb.velocity = new Vector3(UnityEngine.Random.Range(-20f, 20f),
                            UnityEngine.Random.Range(0, 5f),
                            UnityEngine.Random.Range(-20f, 20f));
        }

    }

}
