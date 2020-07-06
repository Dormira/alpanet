using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectAlpacaMenu : MonoBehaviour
{
    public GameObject[] alpacas;
    // Start is called before the first frame update
    void Start()
    {
        alpacas = GameObject.FindGameObjectsWithTag("Alpaca");
    }

    // Update is called once per frame
    void OnGUI()
    {

        foreach(GameObject alpaca in alpacas)
        {
            getSnapshot snapscript = alpaca.GetComponent<getSnapshot>();
            if (GUI.Button(new Rect(25, 0, 25, 25), snapscript.takeSnapshot())){ 
            }
        }


    }


}
