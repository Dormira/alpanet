using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    int grassDensity;
    Vector3[] grasslocs;
    // Start is called before the first frame update
    void Start()
    {
        int[] triangles = MeshGetters.getMeshTriangles(this.gameObject);
        Vector3[] vertices = MeshGetters.getMeshVertices(this.gameObject);

        //Iterate through our triangles array three at a time (one triangle)
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //Convert to vertices
            Vector3 pointA = vertices[triangles[i]];
            Vector3 pointB = vertices[triangles[i + 1]];
            Vector3 pointC = vertices[triangles[i + 2]];

            int area = Geometry.GetTriangleArea(pointA, pointB, pointC);
            //Use geometry function to get area of triangle
            //Multiply area by density to get number of blades
            //Use geometry function to get n random locations
            //Feed these locations into grassLocs so update can render them
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Run through grassLocs 1023 elements at a time and call the batch renderer
        
    }
}
