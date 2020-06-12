using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEditor;

public class placeFenceButton : MonoBehaviour
{
    public Texture kith;
    public Texture ache;
    public Texture buttonIcon;
    // Start is called before the first frame update
    bool fenceMode;
    void Start()
    {
        fenceMode = false;
        //now kith icon + button
        kith = (Texture)AssetDatabase.LoadAssetAtPath("Assets/nowkith.png", typeof(Object));
        ache = (Texture)AssetDatabase.LoadAssetAtPath("Assets/achewood07.jpg", typeof(Object));

        buttonIcon = kith;
    }

    void OnGUI()
    {
   //     UnityEngine.Debug.Log(kith.dimension);   
        if (GUI.Button(new Rect(0, 0, kith.width/10, kith.height/10), buttonIcon))
        {
            fenceMode = !fenceMode;
            if (fenceMode)
            {
                buttonIcon = kith;
            }
            else
            {
                buttonIcon = ache;
            }
        }

    }

    void OnMouseUpAsButton()
    {
        UnityEngine.Debug.Log("CLICKED");
    }
}
 