//Visualstudio loves to automatically import libraries, but not prune them
//TODO: remove the unnecessary ones. they bother me a little
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections.Specialized;
using System;
using System.Security.Cryptography;
using System.Linq;


public class PerturbVertices : MonoBehaviour {
    //This script needs to be placed on the object whose immediate child is the mesh

    void Start() {
        Mesh mesh = getMesh();
        GeometryFunctions.weldVertices(mesh);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("w"))
        {
            Mesh mesh = getMesh();
            //Change the model vertices
            Vector3[] nv = newVertices();
            Vector2[] nuv = mesh.uv;//newUV(nv);
            int[] nt = mesh.triangles;

            mesh.vertices = nv;
            mesh.uv = nuv;
            mesh.triangles = nt;
            mesh.Optimize();
            //Maybe the model should be saved after optimize
            //We have to update the mesh collider too
        }
    }

    Vector3 wiggleVector3(Vector3 vector, float diff)
    {
        Vector3 newVector = new Vector3(UnityEngine.Random.Range(vector[0] - diff, vector[0] + diff),
                                     UnityEngine.Random.Range(vector[1] - diff, vector[1] + diff),
                                     UnityEngine.Random.Range(vector[2] - diff, vector[2] + diff));
        return newVector;
    }

    Vector2[] newUV(Vector3[] vertices)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        return uvs;
    }

    Vector3[] newVertices()
    {
        Mesh mesh = getMesh();
        Vector3[] oldVertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        
        Vector3[] newVertices = new Vector3[oldVertices.Length];

        bool verticesUpdatedSuccessfully = false;
        do
        {
            verticesUpdatedSuccessfully = true;
            //Modify the vertices
            for (int i = 0; i < newVertices.Length; i++)
            {
                newVertices[i] = wiggleVector3(oldVertices[i], 0.1f);
            }

            //Check the triangles for collisions
            for (int i = 0; i < triangles.Count(); i += 3)
            {
                Vector3[] triangleA = triangles.Skip(i).Take(3).Select(x => newVertices[x]).ToArray();
                for (int j = i + 3; j < triangles.Count(); j += 3)
                {
                    Vector3[] triangleB = triangles.Skip(j).Take(3).Select(x => newVertices[x]).ToArray();

                    if (!GeometryFunctions.triangleNonintersectCheck(triangleA, triangleB))
                    {
                        verticesUpdatedSuccessfully = false;
                        break;
                    }
                }
            }
        } while (!verticesUpdatedSuccessfully);

        MeshSerializer.serializeMesh(mesh, this.name);

        return newVertices;
    }

    Mesh getMesh()
    {
        //This function gets the mesh of the object it is attached to. It must be attached to an object with a mesh
        //Not the parent of an object with a mesh
        //Not the child of an object with a mesh
        //The mesh containing object itself
        if (this.GetComponent<MeshFilter>())
        {
            return this.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (this.GetComponent<SkinnedMeshRenderer>())
        {
            return this.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        return null;
    }

}

