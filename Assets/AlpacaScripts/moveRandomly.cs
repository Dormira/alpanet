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
        
        if(Time.time - startTime > 5)
        {
            startTime = Time.time;
            rb.velocity = new Vector3(UnityEngine.Random.Range(-75f, 75f),
                            UnityEngine.Random.Range(0, 75f),
                            UnityEngine.Random.Range(-75f, 75f));
        }

    }

}
