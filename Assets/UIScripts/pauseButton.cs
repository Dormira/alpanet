using UnityEngine;

public class PauseButton : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 25), "PAUSE"))
        {
            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }

    }
}
