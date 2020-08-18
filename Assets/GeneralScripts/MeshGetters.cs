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
        //This needs to go into a gameobject getcomponents file or something
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

}
