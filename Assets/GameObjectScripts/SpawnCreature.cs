using UnityEngine;

public class SpawnCreature : MonoBehaviour
{
    public Object alpacaPrefab;
    int alpacaIndex = 0;

    void Start()
    {
        alpacaPrefab = Resources.Load<GameObject>("Prefabs/AlpacaPrime");
        spawnAlpaca();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            spawnAlpaca();
        }
    }

    void spawnAlpaca()
    {
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
