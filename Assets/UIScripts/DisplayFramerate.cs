using UnityEngine;

public class DisplayFramerate : MonoBehaviour
{
    //If this is activated, displays the framerate in the upper right hand corner of the screen
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 55, 0, 400, 25), (1.0f / Time.deltaTime).ToString());
    }
}
