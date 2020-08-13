using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    GUIStyle tutorialStyle;

    void Start()
    {
        tutorialStyle = new GUIStyle();
        tutorialStyle.fontSize = 50;
        tutorialStyle.alignment = TextAnchor.UpperCenter;
    }
    // Start is called before the first frame update
    void OnGUI()
    {
        string displayText;
        if (SpawnManager.alpacaIndex.Count == 0)
        {
            displayText = "PRESS THE SPACEBAR";
        } 
        else if(SpawnManager.alpacaIndex.Count == 1)
        {
            displayText = "PRESS THE SPACEBAR AGAIN";
        }
        else
        {
            if (Input.GetKeyDown("left") | Input.GetKeyDown("right")){
                Destroy(this);//Self destruct sequence if the user is a good listener
            }
            displayText = "PRESS THE LEFT OR RIGHT ARROW KEY";
        }
        GUI.Label(new Rect(Screen.width/2, Screen.height / 2, 0,0), displayText, tutorialStyle);
    }
}
