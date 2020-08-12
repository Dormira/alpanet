using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectAlpacaMenu : MonoBehaviour
{
    public Texture[] portraits;
    // Start is called before the first frame update

    // Update is called once per frame
    void OnGUI()
    {
        GameObject[] alpacas = GameObject.FindGameObjectsWithTag("Alpaca");
        if (alpacas.Length != portraits.Length)
        {
            refreshAlpacaPortraits();
        }
        for (int i = 0; i < alpacas.Length; i++)
        {

            if (GUI.Button(new Rect(50*i, 25, 50, 50), portraits[i])){
                Camera.main.GetComponent<FollowGameObject>().setTarget(alpacas[i]);
            }
        }

    }
    
    void refreshAlpacaPortraits()
    {
        GameObject[] alpacas = GameObject.FindGameObjectsWithTag("Alpaca");
        portraits = new Texture[alpacas.Length];
        for(int i = 0; i < alpacas.Length; i++)
        {
            Snapshot snapscript = alpacas[i].GetComponent<Snapshot>();
            portraits[i] = snapscript.getSnapshot();
        }
    }

}
