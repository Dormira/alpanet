using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot : MonoBehaviour
{
    Texture2D snapshot;

    public Texture2D getSnapshot()
    {
        UnityEngine.Debug.Log(snapshot);
        if (snapshot == null) {
            StartCoroutine(takeSnapshot());
        }

        if(snapshot != null)
        {
            return snapshot;
        }

        return null;
    }

    public IEnumerator takeSnapshot()
    {
        GameObject target = this.transform.GetChild(1).gameObject;
        GameObject cameraObject = new GameObject(this.name + "_cam");

        Camera cameraComponent = cameraObject.AddComponent(typeof(Camera)) as Camera;

        //Point it at the object and move it 5m away in the

        Texture2D snapshotBuffer = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //cameraComponent.targetTexture = snapshotBuffer;
        yield return new WaitForEndOfFrame();

        

        cameraObject.transform.position = target.transform.position - new Vector3(1.5f, 1.5f, 1.5f);//not at this position but the model position
        cameraComponent.transform.LookAt(target.transform);//look at the model location
        cameraComponent.enabled = true;
        cameraComponent.Render();
        snapshotBuffer.ReadPixels(new Rect(0, 
        0, 
        snapshotBuffer.width, 
        snapshotBuffer.height), 0, 0);
        cameraComponent.enabled = false;
        Camera.main.Render();

        snapshotBuffer.Apply();
        
        this.snapshot = snapshotBuffer;

        Object.Destroy(cameraObject);

    }
}
