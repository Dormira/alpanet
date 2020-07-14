//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Runtime.Versioning;
using UnityEngine;
//using UnityEditor;

public class spawnCreature : MonoBehaviour
{
    public Object prefab;
    int index = 0;

    void Start()
    {
        prefab = Resources.Load<GameObject>("AlpacaPrime");
        spawn();
        spawn();
    }

    void spawn()
    {
        //Instantiate the object
        GameObject child;
        child = Instantiate(prefab) as GameObject;

        GameObject model = child.transform.GetChild(1).gameObject;
        //Make sure that its mesh is divorced from the prefab otherwise wild shit happens that I hate
        //Figure out a better way than this thing yikes
        Mesh oldmesh;
        if (model.GetComponent<MeshFilter>())
        {
            oldmesh = model.GetComponent<MeshFilter>().sharedMesh;
            model.GetComponent<MeshFilter>().sharedMesh = (Mesh)Instantiate(oldmesh);
        }
        else if (model.GetComponent<SkinnedMeshRenderer>())
        {
            oldmesh = model.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            model.GetComponent<SkinnedMeshRenderer>().sharedMesh = (Mesh)Instantiate(oldmesh);
        }

        child.name = "Alpaca" + index.ToString();
        index++;

        model.GetComponent<ColorChanger>().changeColors();
    }

}
