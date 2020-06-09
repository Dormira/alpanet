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

public class wiggle : MonoBehaviour {
    public Mesh model;

    void Start() {
        MeshFilter viewedModelFilter = (MeshFilter)this.GetComponent("MeshFilter");
        model = viewedModelFilter.mesh;
        GeometryFunctions.weldVertices(model);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("w"))
        {
            //Change the model vertices
            Vector3[] nv = newVertices();
            Vector2[] nuv = model.uv;//newUV(nv);
            int[] nt = model.triangles;

            model.vertices = nv;
            model.uv = nuv;
            model.triangles = nt;
            MeshUtility.Optimize(model);
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
        Vector3[] oldVertices = model.vertices;
        int[] triangles = model.triangles;
        
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

                    if (!GeometryFunctions.trianglesDefinitelyDontIntersect(triangleA, triangleB))
                    {
                        verticesUpdatedSuccessfully = false;
                        break;
                    }
                }
            }
        } while (!verticesUpdatedSuccessfully);
        //Save to disk
        Mesh deepcopy = (Mesh)Instantiate(model);
        AssetDatabase.CreateAsset(deepcopy, "Assets/" + this.name + ".asset");

        return newVertices;
    }

}

