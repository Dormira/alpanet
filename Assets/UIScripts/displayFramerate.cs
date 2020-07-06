using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayFramerate : MonoBehaviour
{
    GameObject target;
    string targetLocText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        targetLocText = (1.0f / Time.deltaTime).ToString();

    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 500, 0, 400, 25), targetLocText);
    }
}
