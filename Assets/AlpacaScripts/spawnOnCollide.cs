using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class spawnOnCollide : MonoBehaviour
{
    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        string collisionName = collision.gameObject.name;
        if (collisionName.Contains("Alpaca")){
            UnityEngine.Debug.Log("collided with " + collision.gameObject.name);
        }
            
    }
}
