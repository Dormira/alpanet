using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectAlpacaMenu : MonoBehaviour
{
    public GameObject[] alpacas;
    public Texture[] portraits;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log(alpacas.Length);
    }

    // Update is called once per frame
    void OnGUI()
    {
        alpacas = GameObject.FindGameObjectsWithTag("Alpaca");

        if (alpacas.Length != portraits.Length || alpacas == null)
        {
            refreshAlpacaPortraits();
        }
        for (int i = 0; i < alpacas.Length; i++)
        {
            Texture portrait;
            if(portraits[i] == null)
            {
                portrait = new Texture2D(1, 1);
                refreshAlpacaPortraits();
            }
            else
            {
                portrait = portraits[i];
            }


            if (GUI.Button(new Rect(50*i, 25, 50, 50), portrait)){
                Camera.main.GetComponent<FollowGameObject>().setTarget(alpacas[i]);
            }
        }

    }
    
    void refreshAlpacaPortraits()
    {
        alpacas = GameObject.FindGameObjectsWithTag("Alpaca");
        portraits = new Texture[alpacas.Length];
        for(int i = 0; i < alpacas.Length; i++)
        {
            Snapshot snapscript = alpacas[i].GetComponent<Snapshot>();
            portraits[i] = snapscript.getSnapshot();
        }
    }

}
