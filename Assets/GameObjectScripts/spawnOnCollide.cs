using System.Collections;
using System;
using UnityEngine;

public class SpawnOnCollide : MonoBehaviour
{
    //This should send a signal to the spawnmanager IMO
    void OnCollisionEnter(Collision collision)
    {
        string collisionName = collision.gameObject.name;
        UnityEngine.Debug.Log("collided with " + collision.gameObject.transform.root.gameObject.name);
            
    }
}
