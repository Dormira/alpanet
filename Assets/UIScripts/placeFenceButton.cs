using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.IO;
using UnityEngine;
using UnityEditor;

public class placeFenceButton : MonoBehaviour
{
    public Texture2D kith;
    public Texture2D ache;
    public Texture buttonIcon;
    // Start is called before the first frame update
    bool fenceMode;

    void Start()
    {
        fenceMode = true;
        //now kith icon + button
        kith = new Texture2D(2,2);
        kith.LoadImage(File.ReadAllBytes(Path.Combine(Application.dataPath,"Images/nowkith.png")));
        ache = new Texture2D(2, 2);
        ache.LoadImage(File.ReadAllBytes(Path.Combine(Application.dataPath, "Images/achewood07.jpg")));

        buttonIcon = kith;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 25, 25), buttonIcon))
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
 