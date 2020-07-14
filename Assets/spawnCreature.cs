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
        GameObject child;
        child = Instantiate(prefab) as GameObject;
        child.name = "Alpaca" + index.ToString();
        index++;
        GameObject model = child.transform.GetChild(1).gameObject;
        model.GetComponent<ColorChanger>().changeColors();
    }

}
