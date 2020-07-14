using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorChanger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("up"))
        {
            changeColors();
        }
    }

    public void changeColors()
    {
        Mesh oldmesh = null;
        //Figure out a better way than this thing yikes
        if (GetComponent<MeshFilter>())
        {
            oldmesh = GetComponent<MeshFilter>().sharedMesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            oldmesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Mesh newmesh = (Mesh)Instantiate(oldmesh);

        Color colorStart = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            1f);
        Color colorEnd = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
           1f);

        Vector3[] vertices = oldmesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(colorStart, colorEnd, vertices[i].y);


        newmesh.vertices = vertices;
        newmesh.colors = colors;


        if (GetComponent<MeshFilter>())
        {
            GetComponent<MeshFilter>().sharedMesh = newmesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            GetComponent<SkinnedMeshRenderer>().sharedMesh = newmesh;
        }
        //Destroy(newmesh);
        
        //DestroyImmediate(oldmesh, true);

        //Remove old mesh
    }
}
