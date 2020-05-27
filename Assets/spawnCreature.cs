using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class spawnCreature : MonoBehaviour
{
    public GameObject Cube;
// UnityEvent spawnCreatureEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        Cube = GameObject.Find("Cube");

    //    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
     //   foreach (GameObject go in allObjects)
     //   {
          //  UnityEngine.Debug.Log(go);
        spawn();
      //  }
        //  spawnCreatureEvent.addListener();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void spawn()
    {
        Instantiate(Cube);
    }

}
