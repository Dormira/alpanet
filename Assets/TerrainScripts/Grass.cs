using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grass : MonoBehaviour
{
    List<Matrix4x4[]> grassTransforms;//These are the transform matrices of each blade of grass
    Mesh mesh;
    Material grassRenderMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //Get the mesh and the material from the 
        this.mesh = MeshGetters.getQuadMesh(1f, 5f);
        this.grassRenderMaterial = Resources.Load<Material>("Materials/pink");
        grassTransforms = new List<Matrix4x4[]>();

        float grassDensity = 0.1f;//1 blade of grass per area unit

        int[] triangles = MeshGetters.getMeshTriangles(this.gameObject);
        Vector3[] vertices = MeshGetters.getMeshVertices(this.gameObject);

        Vector3[] curVertices = new Vector3[3];

        int gtbi = 0;
        Matrix4x4[] grassTransformBuffer = new Matrix4x4[1023];
        //Iterate through our triangles array three at a time (one triangle)
        for (int i = 0; i < triangles.Length; i += 3)
        {
            
            //Convert to vertices
            curVertices[0] = Vector3.Scale(vertices[triangles[i]] , transform.localScale) + transform.position;
            curVertices[1] = Vector3.Scale(vertices[triangles[i+1]], transform.localScale) + transform.position;
            curVertices[2] = Vector3.Scale(vertices[triangles[i + 2]], transform.localScale) + transform.position;
            
            float area = GeometryFunctions.getTriangleArea(curVertices);

            int numBlades = (int)(area * grassDensity);
            Vector3[] grassLocs = GeometryFunctions.getNLocationsOnTriangle(curVertices, numBlades);

            for (int j = 0; j < grassLocs.Length; j++)
            {
                if(gtbi == 1023)
                {
                    gtbi = 0;
                    Matrix4x4[] gtBufferCopy = (Matrix4x4[])grassTransformBuffer.Clone();
                    grassTransforms.Add(gtBufferCopy);
                }
                Quaternion rotationQuat = Quaternion.FromToRotation(transform.up, GeometryFunctions.getTriangleNormal(curVertices));
                grassTransformBuffer[gtbi] = Matrix4x4.TRS(grassLocs[j], rotationQuat, new Vector3(1,1,1));
                gtbi++;
            }
        }
        //Get them leftovers
        grassTransforms.Add(grassTransformBuffer);//this should be truncated and then added to the dynamic list based on the current j
        //will deal with it later
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < grassTransforms.Count; i++)
        {
            Graphics.DrawMeshInstanced(this.mesh,
                0,
                this.grassRenderMaterial,
                grassTransforms[i]);

        }
    }
}