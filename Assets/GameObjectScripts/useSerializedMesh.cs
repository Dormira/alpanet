using UnityEngine;

public class UseSerializedMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        //deserializeMesh returns a default mesh, if the requested one doesn't exist
        Mesh loadedMesh = MeshSerializer.deserializeMesh(this.name);
      
        mesh.triangles = loadedMesh.triangles;
        mesh.vertices = loadedMesh.vertices;
        mesh.colors = loadedMesh.colors;

        //UPDATE THE MESH COLLIDER
        DestroyImmediate(this.GetComponent<MeshCollider>());
        var collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        collider.convex = true;
    }
}
