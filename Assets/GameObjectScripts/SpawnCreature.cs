using UnityEngine;

public class SpawnCreature : MonoBehaviour
{
    /*
     * This is really a spawnmanager specific script, 
     * but that may not stay the case so it makes sense to me to keep it with the generic GameObject scripts for now
     */
    public int alpacaIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown("space") & alpacaIndex < 10)
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
        alpacaObject.name = "Alpaca" + alpacaIndex.ToString();
        alpacaIndex++;

        //Set the camera to follow the new alpaca
        Camera.main.GetComponent<FollowGameObject>().setTarget(alpacaObject);

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
