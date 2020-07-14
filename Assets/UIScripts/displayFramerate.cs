using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayFramerate : MonoBehaviour
{
    GameObject target;
    string targetLocText;


    // Update is called once per frame
    void Update()
    {
        targetLocText = (1.0f / Time.deltaTime).ToString();

    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 55, 0, 400, 25), targetLocText);
    }
}
