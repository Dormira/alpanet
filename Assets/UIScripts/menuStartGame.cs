using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuStartGame : MonoBehaviour
{
    UnityEngine.AsyncOperation loadGameOperation;
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width/2)-100, (Screen.height/2)-25, 200, 50), "PRESS THIS PUTTON"))
        {
            loadGameOperation.allowSceneActivation = true;
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;

        loadGameOperation = SceneManager.LoadSceneAsync("game");
        loadGameOperation.allowSceneActivation = false;
    }
}
