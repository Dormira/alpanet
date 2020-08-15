using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot : MonoBehaviour
{
    Texture2D snapshot;

    public Texture2D getSnapshot()
    {
        if (snapshot == null) {
            takeSnapshot();
        }

        return snapshot;

    }

    public void takeSnapshot()
    {
        //Get the target and the camera
        GameObject target = transform.Find("AlpacaMesh").gameObject;
        GameObject cameraObject = Camera.main.gameObject;
        //Save the camera's old transform so we can put it back
        Transform oldTransform = cameraObject.transform;
        //Change the transform so we get the shot we want
        cameraObject.transform.position = target.transform.position - new Vector3(3f, 3f, 3f);
        cameraObject.transform.LookAt(target.transform);
        //Take the shot
        snapshot = new Texture2D(Screen.width/3, 
            Screen.height/3, 
            TextureFormat.RGB24, 
            false);
        cameraObject.GetComponent<Camera>().Render();
        snapshot.ReadPixels(new Rect(Screen.width/3,
            Screen.height/3,
        snapshot.width,
        snapshot.height), 0, 0);
        snapshot.Apply();
        //Put our camera back
        cameraObject.transform.position = oldTransform.position;
        cameraObject.transform.rotation = oldTransform.rotation;

    }

}
