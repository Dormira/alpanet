using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class spawnCreature : MonoBehaviour
{
    public GameObject creature;

    void Start()
    {
        creature = GameObject.Find("AlpacaPrime");
        spawn();
    }

    void spawn()
    {
        Instantiate(creature);
    }

}
