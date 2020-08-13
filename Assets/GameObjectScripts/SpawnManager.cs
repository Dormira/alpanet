using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /*
     * This is really a spawnmanager specific script, 
     * but that may not stay the case so it makes sense to me to keep it with the generic GameObject scripts for now
     */
    public int currentIndex = 10;
    public static List<string> alpacaIndex = new List<string>();//int->string

    void Update()
    {
        if (Input.GetKeyDown("space") & alpacaIndex.Count < 10)
        {
            spawnAlpaca();
        }
    }

    void spawnAlpaca()
    {
        Object alpacaPrefab = Resources.Load<GameObject>("Prefabs/AlpacaPrime");
        //Instantiate the new alpaca
        GameObject alpacaObject = Instantiate(alpacaPrefab) as GameObject;

        //Give the alpaca a unique name
        alpacaIndex.Add(currentIndex.ToString());
        alpacaObject.name = currentIndex.ToString();
        currentIndex++;

        //Set the camera to follow the new alpaca
        Camera.main.GetComponent<FollowGameObject>().setTarget(alpacaIndex.Count-1);

        changeColors(alpacaObject);
    }

    void changeColors(GameObject alpacaObject)
    {
        //Generate a random color
        Color color = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            1f);

        //Get the mesh child of the alpaca, then the mesh renderer component of the mesh
        GameObject alpacaMesh = alpacaObject.transform.Find("AlpacaMesh").gameObject;
        var alpacaRenderer = alpacaMesh.GetComponent<SkinnedMeshRenderer>();

        //Set the albedo color of the material
        alpacaRenderer.material.SetColor("_Color", color);
    }
}
