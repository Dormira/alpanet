﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 10000f;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //TODO: disentangle this from specific object
        target = GameObject.Find("Alpaca1");

    } 

    // Update is called once per frame
    void Update()
    {
        //So the actual target position should be
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if(distanceToTarget > 5f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, smoothTime);
        }
        transform.LookAt(target.transform);

    }
}