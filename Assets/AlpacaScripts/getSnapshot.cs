using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getSnapshot : MonoBehaviour
{
   // GameObject cam;
    Camera cam;
    void Start()
    {
        GameObject tempcam = new GameObject(this.name + "_cam");
        cam = tempcam.AddComponent(typeof(Camera)) as Camera;
    }

    public Texture2D takeSnapshot()
    {

        //Point it at the object and move it 5m away in the
        UnityEngine.Debug.Log(this.transform.position[0]);
        cam.transform.position = this.transform.position - new Vector3(0, 0, 5);
        cam.transform.LookAt(this.transform);
        Texture2D image = new Texture2D(100, 100);
        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
       // yield return frameEnd;
        //     Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
        image.Apply();
        return image;


    }
}
