using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartGame : MonoBehaviour
{
    Dictionary<string, UnityEngine.AsyncOperation> loadSceneOperations = new Dictionary<string, UnityEngine.AsyncOperation>();

    void Start()
    {
        //If the main menu is loaded, we should start loading the game behind the scenes to minimize wait times
        //But none of our scenes are large enough for this to matter yet
        loadSceneOperations.Add("flatWorld", new UnityEngine.AsyncOperation());
        loadSceneOperations.Add("roundWorld", new UnityEngine.AsyncOperation());
    }

    void OnGUI()
    {
        int idx = 0;
        foreach (KeyValuePair<string, UnityEngine.AsyncOperation> entry in loadSceneOperations)
        {
            idx++;
            if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - (50*idx), 200, 50), entry.Key))
            {
                StartCoroutine(LoadScene(entry.Key));
            }
        }

    }

    //This function loads the game scene, but the start button activates it
    IEnumerator LoadScene(string sceneName)
    {
        yield return null;
        loadSceneOperations[sceneName] = SceneManager.LoadSceneAsync(sceneName);
        loadSceneOperations[sceneName].allowSceneActivation = true;
    }

    //Unloads a scene by name. Unity hates having more than one scene loaded
    IEnumerator unloadScene(string sceneName)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(sceneName);
    }


}
