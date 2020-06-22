using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayObjectLocation : MonoBehaviour
{
    GameObject target;
    string targetLocText;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Alpaca1");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = target.transform.position;
        targetLocText = pos[0].ToString()+", "+pos[1].ToString() + ", "+pos[2];

    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width-400, 0, 400, 25), targetLocText);
        GUI.Label(new Rect(Screen.width - 400, 25, 400, 50), "LOCATION: "+Application.dataPath);
    }
}
