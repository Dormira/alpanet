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

            //    model.Clear();

            model.vertices = nv;
            model.uv = nuv;
            model.triangles = nt;
            MeshUtility.Optimize(model);
        }

        if (Input.GetKeyDown("q"))
        {
            Vector3[] triangleA = new Vector3[3];
            triangleA[0] = new Vector3(-0.5f, 0.5f, 0.4f);
            triangleA[1] = new Vector3(0.4f, 0.5f, 0.5f);
            triangleA[2] = new Vector3(0.4f, -0.5f, 0.4f);


            Vector3[] triangleB = new Vector3[3];
            triangleB[0] = new Vector3(-0.5f, 0.5f, -0.5f);
            triangleB[1] = new Vector3(0.4f, 0.5f, -0.6f);
            triangleB[2] = new Vector3(0.4f, 0.5f, 0.5f);

            GeometryFunctions.trianglesDefinitelyDontIntersect(triangleA, triangleB);

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
            //    int[] triangleA = triangles.Skip(i).Take(3).ToArray();
                Vector3[] triangleA = triangles.Skip(i).Take(3).Select(x => newVertices[x]).ToArray();
                for (int j = i + 3; j < triangles.Count(); j += 3)
                {
               //     Vector3 triangleB = triangles.Skip(j).Take(3).ToArray();
                    Vector3[] triangleB = triangles.Skip(j).Take(3).Select(x => newVertices[x]).ToArray();

                    if (!GeometryFunctions.trianglesDefinitelyDontIntersect(triangleA, triangleB))
                    {
                        verticesUpdatedSuccessfully = false;
                        break;
                    }
                }
            }
        } while (!verticesUpdatedSuccessfully);

        return newVertices;
    }
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
    /*
    Vector3[] newVertices_deprecated()
    {
        Vector3[] oldVertices = model.vertices;
        Vector3[] freshVertices = new Vector3[oldVertices.Count()];

        Dictionary<Vector3, Vector3> vertexConversions = new Dictionary<Vector3, Vector3>();


        bool updatedTriangle;
        for (int i = 0; i < oldVertices.Count(); i += 3)
        {
              do
              {
                
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
                (newa, newb, newc, newd) = GeometryFunctions.planeEquation(triangle);
                for (int j = 0; j <= i; j += 3)
                {
                    invalidTriangle = false;
                    Vector3 vertexA = freshVertices[j];
                    Vector3 vertexB = freshVertices[j+1];
                    Vector3 vertexC = freshVertices[j+2];

                    //Is the current triangle in freshVertices on the same side of the plane of our newly generated triangle?
                    if (GeometryFunctions.pointsOnSameSide(vertexA, vertexB, newa, newb, newc, newd) && GeometryFunctions.pointsOnSameSide(vertexA, vertexC, newa, newb, newc, newd))
                    {
                        continue;
                    }
                    else
                    {
                        float olda, oldb, oldc, oldd;
                        (olda, oldb, oldc, oldd) = GeometryFunctions.planeEquation(freshVertices.Skip(j).Take(3).ToArray());
                        if (GeometryFunctions.pointsOnSameSide(triangle[0], triangle[1], olda, oldb, oldc, oldd) && GeometryFunctions.pointsOnSameSide(triangle[0], triangle[2], olda, oldb, oldc, oldd))
                        {
                            continue;
                        }
                        else
                        {
                            invalidTriangle = true;
                        }

                    }
                }
                if(!invalidTriangle){
                    freshVertices[i] = triangle[0];
                    freshVertices[i + 1] = triangle[1];
                    freshVertices[i + 2] = triangle[2];
                    updatedTriangle = true;
                    
                }
            } while (!updatedTriangle );
            


        }
        return freshVertices;
    }

    */
}

