using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class changeColors : MonoBehaviour
{
    Renderer rend;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshFilter>())
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }
        else if(GetComponent<SkinnedMeshRenderer>())
        {
            mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
        Color colorStart = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            1f);
        Color colorEnd = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
           1f);

        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(colorStart, colorEnd, vertices[i].y);

        mesh.colors = colors;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
