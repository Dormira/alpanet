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
//using System.Runtime.Remoting.Messaging;

public class wiggle : MonoBehaviour {
    public Mesh model;
    private Vector3 targetLocation;
    public float speed = 10.0f;//idk what 5 means...
                               // Use this for initialization
    void Start() {
        MeshFilter viewedModelFilter = (MeshFilter)this.GetComponent("MeshFilter");
        model = viewedModelFilter.mesh;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("w"))
        {
            //Change the model vertices
            Vector3[] nv = newVertices();
            Vector2[] nuv = model.uv;//newUV(nv);
            int[] nt = model.triangles;

            //    model.Clear();

            model.vertices = nv;
            model.uv = nuv;
            model.triangles = nt;
            MeshUtility.Optimize(model);
        }
        //        moveToTargetLocation();
    }

    void moveToTargetLocation()
    {
        if (this.transform.position == targetLocation)
        {
            Vector3 source_vertex = this.transform.position;

            targetLocation = new Vector3(UnityEngine.Random.Range(source_vertex[0] - 10f, source_vertex[0] + 10f),
                                     UnityEngine.Random.Range(source_vertex[1] - 10f, source_vertex[1] + 10f),
                                     UnityEngine.Random.Range(source_vertex[2] - 10f, source_vertex[2] + 10f));
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(this.transform.position, targetLocation, step);
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
        Vector3[] freshVertices = new Vector3[oldVertices.Count()];

        Dictionary<Vector3, Vector3> vertexConversions = new Dictionary<Vector3, Vector3>();

        //Vertices are going to have to be added three at a time because that's what forms a new triangle
        //For every three vertices in vertices
        //if vertex exists in vertex_conversions then new vertex is the conversion
        //otherwise random offset the vertex and add to dict (BUT DON'T MARRY IT TO THE DICTIONARY)
        //For every three vertices in newVertices
        //does the triangle we just generated intersect?
        //compute plane equation of new triangle//DONEZO
        //are all points of old triangle on the same side, if so not intersecting //DONEZO
        //compute plane equation of old triangle//DONEZO
        //are all points of new triangle on the same side as old triangle, if so not intersecting
        //compute intersection line and project onto largest axis (what do this mean)
        //compute the intervals for each triangle (whats an interval)
        //intersect the intervals
        //if no, add to newVertices
        //otherwise try again
        bool updatedTriangle;
        for (int i = 0; i < oldVertices.Count(); i += 3)
        {
              do
              {
                UnityEngine.Debug.Log((i + 3) + "/"+ oldVertices.Count());
                
                Vector3[] triangle = oldVertices.Skip(i).Take(3).ToArray();

                for (int j = 0; j < triangle.Count(); j += 1)
                {
                    Vector3 vertex = triangle[j];
                    if (vertexConversions.ContainsKey(vertex))
                    {
                        triangle[j] = vertexConversions[vertex];
                    }
                    else
                    {
                        triangle[j] = wiggleVector3(vertex, 0.1f);
                        vertexConversions[vertex] = triangle[j];
                    }
                }


                bool invalidTriangle = false;
                updatedTriangle = false;
                if(i == 0)
                {
                    updatedTriangle = true;
                }
                float newa, newb, newc, newd;
                (newa, newb, newc, newd) = PlaneEquation(triangle);
                for (int j = 0; j <= i; j += 3)//j+3 or i-3 for middle part
                {
                 //   UnityEngine.Debug.Log((j + 3) + "/INNER/" + i);
                    invalidTriangle = false;
                    Vector3 vertexA = freshVertices[j];
                    Vector3 vertexB = freshVertices[j+1];
                    Vector3 vertexC = freshVertices[j+2];

                    //Is the current triangle in freshVertices on the same side of the plane of our newly generated triangle?
                    if (pointsOnSameSide(vertexA, vertexB, newa, newb, newc, newd) && pointsOnSameSide(vertexA, vertexC, newa, newb, newc, newd))
                    {
                        continue;
                    }
                    else
                    {
                        //Is our newly generated triangle on the same side of the plane as our current triangle?
                        float olda, oldb, oldc, oldd;
                        (olda, oldb, oldc, oldd) = PlaneEquation(freshVertices.Skip(j).Take(3).ToArray());
                        if (pointsOnSameSide(triangle[0], triangle[1], olda, oldb, oldc, oldd) && pointsOnSameSide(triangle[0], triangle[2], olda, oldb, oldc, oldd))
                        {
                            continue;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Invalid Triangle");
                            invalidTriangle = true;
                        }

                   //     UnityEngine.Debug.Log("BEEFED IT ");
                    }
                }
                if(!invalidTriangle){
                    UnityEngine.Debug.Log("Triangle Updated");
                    freshVertices[i] = triangle[0];
                    freshVertices[i + 1] = triangle[1];
                    freshVertices[i + 2] = triangle[2];
                  //  i += 3;
                    updatedTriangle = true;
                    
                }
                //don't break the loop until we've updated a triangle
            } while (!updatedTriangle );
            //Put a while len(newvertices) != i (+3)
            //Generate a new triangle
            


        }
        return freshVertices;
    }
    //TODO move general math functions to their own file
    bool pointsOnSameSide(Vector3 pointA, Vector3 pointB, float a, float b, float c, float d)
    {
        float res1 = pointA[0] * a + pointA[1] * b + pointA[2] * c + d;
        float res2 = pointB[0] * a + pointB[1] * b + pointB[2] * c + d;
        if(res1*res2 > 0)
        {
            return true;
        }
       // UnityEngine.Debug.Log("INCORRECT TRIANGLE GENERATED");
        return false;
    }

    (float, float, float, float) PlaneEquation(Vector3[] triangle)
    {
        Vector3 t0 = triangle[0];
        Vector3 t1 = triangle[1];
        Vector3 t2 = triangle[2];

        Vector3 ab = t1 - t0;
        Vector3 ac = t2 - t0;

        Vector3 normal = Vector3.Cross(ab, ac);

        float a = ab[1] * ac[2] - ac[2] * ab[2];
        float b = ac[0] * ab[2] - ab[0] * ac[2];
        float c = ab[0] * ac[2] - ab[1] * ac[0];

        float d = -(a) * t0[0] - (b) * t0[1] - (c) * t0[2];

        return (a, b, c, d);
    }
}

