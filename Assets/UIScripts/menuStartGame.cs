using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartGame : MonoBehaviour
{
    UnityEngine.AsyncOperation loadGameOperation;
    void Start()
    {
        //If the main menu is loaded, we should start loading the game behind the scenes to minimize wait times
        StartCoroutine(LoadScene());
    }

    void OnGUI()
    {
        if(!(loadGameOperation.progress >= 0.9)){
            if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 25, 200, 50), "START GAME"))
            {
                loadGameOperation.allowSceneActivation = true;
            }
        }
        else
        {

            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "LOADING...");
        }
        //Start game button, activates pre-loaded scene
    }

    //This function loads the game scene, but the start button activates it
    IEnumerator LoadScene()
    {
        yield return null;

        loadGameOperation = SceneManager.LoadSceneAsync("game");
        loadGameOperation.allowSceneActivation = false;
    }
}
