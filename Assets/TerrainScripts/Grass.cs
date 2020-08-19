using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grass : MonoBehaviour
{
    List<Matrix4x4[]> grassTransforms;//These are the transform matrices of each blade of grass
    Mesh mesh;
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("SCALING BY "+ transform.localScale);
        //Get the mesh and the material from the 
        this.mesh = MeshGetters.getCubeMesh();
        //this.material = MeshGetters.getMeshMaterial(this.gameObject);
        this.material = Resources.Load<Material>("Materials/pink");
        grassTransforms = new List<Matrix4x4[]>();
        UnityEngine.Debug.Log(grassTransforms.Count);

        float grassDensity = 1f;//1 blade of grass per area unit

        int[] triangles = MeshGetters.getMeshTriangles(this.gameObject);
        Vector3[] vertices = MeshGetters.getMeshVertices(this.gameObject);

        Vector3[] curVertices = new Vector3[3];

        Matrix4x4[] grassTransformBuffer = new Matrix4x4[1023];

        int gtbi = 0;
        //Iterate through our triangles array three at a time (one triangle)
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //Convert to vertices
            curVertices[0] = Vector3.Scale(vertices[triangles[i]] , transform.localScale) + transform.position;
            curVertices[1] = Vector3.Scale(vertices[triangles[i+1]], transform.localScale) + transform.position;
            curVertices[2] = Vector3.Scale(vertices[triangles[i + 2]], transform.localScale) + transform.position;
            
            /*
            curVertices[0] = Vector3.Scale(vertices[triangles[i]];
            curVertices[1] = Vector3.Scale(vertices[triangles[i + 1]], transform.localScale);
            curVertices[2] = Vector3.Scale(vertices[triangles[i + 2]], transform.localScale);
            */
            float area = GeometryFunctions.getTriangleArea(curVertices);

            int numBlades = (int)(area * grassDensity);
            numBlades++;//DEBUG
            Vector3[] grassLocs = GeometryFunctions.getNLocationsOnTriangle(curVertices, numBlades);
            if (grassLocs.Length > 0)
            {
                UnityEngine.Debug.Log(grassLocs[0]);
            }
            for (int j = 0; j < grassLocs.Length; j++)
            {
                if(gtbi == 1023)
                {
                    gtbi = 0;
                    grassTransforms.Add(grassTransformBuffer);
                }
                grassTransformBuffer[gtbi] = Matrix4x4.Translate(grassLocs[j]);
                UnityEngine.Debug.Log(grassTransformBuffer[gtbi]);
                gtbi++;
            }
        }
        //Get them leftovers
        grassTransforms.Add(grassTransformBuffer);
        UnityEngine.Debug.Log(grassTransformBuffer[0]);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < grassTransforms.Count; i++)
        {
            Graphics.DrawMeshInstanced(this.mesh,
                0,
                this.material,
                grassTransforms[i]);
        }
        
    }
}