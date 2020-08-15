using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    float pullRadius = 1000;
    float pullForce = 100f;

    // Update is called once per frame
    void Update()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius))
        {
            Vector3 forceDirection = transform.position - collider.transform.position;

            collider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
        }
    }
}
