using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGetters
{
    //These are just a bunch of getters 
    //so that I don't have to think about whether a model is using a static mesh filter or a skinned mesh renderer
    //
    public static Mesh getMesh(GameObject objectWithMesh)
    {
        //This function gets the mesh of the object passed to it
        if (objectWithMesh.GetComponent<MeshFilter>())
        {
            return objectWithMesh.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (objectWithMesh.GetComponent<SkinnedMeshRenderer>())
        {
            return objectWithMesh.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        return null;
    }

    public static Mesh getCubeMesh()
    {
        float height = 5;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[8]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (height, 0, 0),
            new Vector3 (height, height, 0),
            new Vector3 (0, height, 0),
            new Vector3 (0, height, height),
            new Vector3 (height, height, height),
            new Vector3 (height, 0, height),
            new Vector3 (0, 0, height)
        };
        mesh.vertices = vertices;

        int[] tris = new int[36]
        {
            0, 2, 1, //face front
	        0, 3, 2,
            2, 3, 4, //face top
	        2, 4, 5,
            1, 2, 5, //face right
	        1, 5, 6,
            0, 7, 4, //face left
	        0, 4, 3,
            5, 4, 7, //face back
	        5, 7, 6,
            0, 6, 7, //face bottom
	        0, 1, 6
        };
        mesh.triangles = tris;

        mesh.Optimize();
        mesh.RecalculateNormals();


        return mesh;
    }

    public static Mesh getQuadMesh(float width = 1f, float height = 1f)
    {

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0,0,0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            0,2,1,2,3,1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        return mesh;
    }

    public static Mesh getTriMesh(float width = 1f, float height = 1f)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3]
        {
            new Vector3(0,height,0),
            new Vector3(width/2, 0, 0),
            new Vector3(-width/2, 0, 0),
        };
        mesh.vertices = vertices;

        int[] tris = new int[3]
        {
            0,1,2
        };
        mesh.triangles = tris;

        mesh.Optimize();
        mesh.RecalculateNormals();

        return mesh;
    }

    public static int[] getMeshTriangles(GameObject objectWithMesh)
    {
        if (objectWithMesh.GetComponent<MeshFilter>())
        {
            return objectWithMesh.GetComponent<MeshFilter>().sharedMesh.triangles;
        }
        else if (objectWithMesh.GetComponent<SkinnedMeshRenderer>())
        {
            return objectWithMesh.GetComponent<SkinnedMeshRenderer>().sharedMesh.triangles;
        }

        return null;
    }

    public static Vector3[] getMeshVertices(GameObject objectWithMesh)
    {
        if (objectWithMesh.GetComponent<MeshFilter>())
        {
            return objectWithMesh.GetComponent<MeshFilter>().sharedMesh.vertices;
        }
        else if (objectWithMesh.GetComponent<SkinnedMeshRenderer>())
        {
            return objectWithMesh.GetComponent<SkinnedMeshRenderer>().sharedMesh.vertices;
        }

        return null;
    }

    public static Material getMeshMaterial(GameObject objectWithMesh)
    {
        return objectWithMesh.GetComponent<MeshRenderer>().material;
    }

}
