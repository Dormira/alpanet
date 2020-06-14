using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useSerializedMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Mesh loadedMesh = meshSerializer.deserializeMesh(this.name);

        mesh.vertices = loadedMesh.vertices;
        mesh.triangles = loadedMesh.triangles;
        mesh.colors = loadedMesh.colors;
    }

}
