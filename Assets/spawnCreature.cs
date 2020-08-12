using UnityEngine;

public class spawnCreature : MonoBehaviour
{
    public Object prefab;
    int index = 0;

    void Start()
    {
        //This is our ur-alpaca. The blueprint. The template.
        //There is nothing wrong with this alpaca
        prefab = Resources.Load<GameObject>("Prefabs/AlpacaPrime");
        //So spawn two of them
        spawn();
        spawn();
    }

    void spawn()
    {
        //Instantiate the object
        GameObject child;
        child = Instantiate(prefab) as GameObject;

        GameObject model = child.transform.GetChild(1).gameObject;
        //The model will either be a static mesh or a rigged mesh
        Mesh oldmesh;
        if (model.GetComponent<MeshFilter>())
        {
            oldmesh = model.GetComponent<MeshFilter>().sharedMesh;
            model.GetComponent<MeshFilter>().sharedMesh = (Mesh)Instantiate(oldmesh);
        }
        else if (model.GetComponent<SkinnedMeshRenderer>())
        {
            oldmesh = model.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            model.GetComponent<SkinnedMeshRenderer>().sharedMesh = (Mesh)Instantiate(oldmesh);
        }

        child.name = "Alpaca" + index.ToString();
        index++;

        model.GetComponent<ColorChanger>().changeColors();
    }

}
