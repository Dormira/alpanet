using UnityEngine;
using System;

[System.Serializable]
public class PerturbVertices : MonoBehaviour {
    public bool saveModel;
    public float perturbationAmount;
    //This script needs to be placed on the object whose immediate child is the mesh
    void Start() {
        Mesh mesh = MeshGetters.getMesh(this.gameObject);
        //So my suspicion is that the vertex welding doesn't work on skinned mesh renderers because we're not associating the modified vertices with the appropriate bone anymore.
        GeometryFunctions.weldVertices(mesh);
        perturbMesh();
    }

    void perturbMesh()
    {
        Mesh mesh = MeshGetters.getMesh(this.gameObject);
        //Change the model vertices
        Vector3[] nv = newVertices();
        Vector2[] nuv = mesh.uv;
        int[] nt = mesh.triangles;

        mesh.vertices = nv;
        mesh.uv = nuv;
        mesh.triangles = nt;
        mesh.Optimize();


        if (this.gameObject.GetComponent<MeshCollider>())
        {
            //UPDATE THE MESH COLLIDER. This code gets used in UseSerializedMesh. Should it be its own thing?
            DestroyImmediate(this.gameObject.GetComponent<MeshCollider>());
            var collider = this.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
        }
    }

    void wiggleVector3(ref Vector3 vector)
    {
        vector = new Vector3(UnityEngine.Random.Range(vector[0] - perturbationAmount, vector[0] + perturbationAmount),
                                     UnityEngine.Random.Range(vector[1] - perturbationAmount, vector[1] + perturbationAmount),
                                     UnityEngine.Random.Range(vector[2] - perturbationAmount, vector[2] + perturbationAmount));
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
        
        Mesh mesh = MeshGetters.getMesh(this.gameObject);
        int[] triangles = mesh.triangles;

        Vector3[] triangleA = new Vector3[3];//Lets not allocate new vector3s so much
        Vector3[] triangleB = new Vector3[3];

        bool verticesUpdatedSuccessfully;
        Vector3[] newVertices;
        do
        {
            newVertices = mesh.vertices;
            verticesUpdatedSuccessfully = true;
            //Modify the vertices
            for (int i = 0; i < newVertices.Length; i++)
            {
                wiggleVector3(ref newVertices[i]);
            }

            //Check the triangles for collisions
            for (int i = 0; i < triangles.Length; i += 3)
            {
                triangleA[0] = newVertices[triangles[i]];
                triangleA[1] = newVertices[triangles[i + 1]];
                triangleA[2] = newVertices[triangles[i + 2]];
               
                for (int j = i + 3; j < triangles.Length; j += 3)
                {
                    triangleB[0] = newVertices[triangles[j]];
                    triangleB[1] = newVertices[triangles[j + 1]];
                    triangleB[2] = newVertices[triangles[j + 2]];

                    if (!GeometryFunctions.triangleNonintersectCheck(triangleA, triangleB))
                    {
                        verticesUpdatedSuccessfully = false;
                        break;
                    }
                }
            }
        } while (!verticesUpdatedSuccessfully);

        if (this.saveModel) {
            MeshSerializer.serializeMesh(mesh, this.name);
        }


        return newVertices;
    }





}

