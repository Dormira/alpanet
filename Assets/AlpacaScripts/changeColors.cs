using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColors : MonoBehaviour
{
    Color colorStart = Color.red;
    Color colorEnd = Color.green;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
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
