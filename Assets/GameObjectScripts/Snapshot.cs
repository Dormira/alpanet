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
        GameObject target = this.transform.Find("AlpacaMesh").gameObject;
        GameObject cameraObject = GameObject.Find("MainCamera");
        //Save the camera's old transform so we can put it back
        Transform oldTransform = cameraObject.transform;
        //Change the transform so we get the shot we want
        cameraObject.transform.position = target.transform.position - new Vector3(1.5f, 1.5f, 1.5f);
        cameraObject.transform.LookAt(target.transform);
        //Take the shot
        Texture2D snapshotBuffer = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraObject.GetComponent<Camera>().Render();
        snapshotBuffer.ReadPixels(new Rect(0,
        0,
        snapshotBuffer.width,
        snapshotBuffer.height), 0, 0);
        snapshotBuffer.Apply();
        //Put our camera back
        cameraObject.transform.position = oldTransform.position;
        cameraObject.transform.rotation = oldTransform.rotation;
        //do we have to re-render to make it seamless?
        this.snapshot = snapshotBuffer;

    }

}
