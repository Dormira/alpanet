using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GeometryFunctions{
    //Calculates and returns the plane equation of the given triangle
    public static Vector4 planeEquation(Vector3[] triangle){

    Vector3 t0 = triangle[0];
    Vector3 t1 = triangle[1];
    Vector3 t2 = triangle[2];

    Vector3 ab = t1 - t0;
    Vector3 ac = t2 - t0;

    Vector3 normal = Vector3.Cross(ab, ac);

    float d = normal[0] * t0[0] + normal[1] * t0[1] + normal[2] * t0[2];

    return new Vector4(normal[0], normal[1], normal[2], d);
}
    //Calculates whether or not pointA and pointB are on the same side of plane equation ax+by+cz = d
    public static bool pointsOnSameSide(Vector3 pointA, Vector3 pointB, Vector4 planeEquation)
    {
        float a = planeEquation[0];
        float b = planeEquation[1];
        float c = planeEquation[2];
        float d = planeEquation[3];

        if (pointA == pointB)
        {
            return true;
        }

        float res1 = pointA[0] * a + pointA[1] * b + pointA[2] * c + d;
        float res2 = pointB[0] * a + pointB[1] * b + pointB[2] * c + d;
        if (res1 * res2 >= 0)
        {
            return true;
        }
        return false;
    }

    //This function takes THREE POINTS, NOT THREE INDICES
    public static bool trianglesDefinitelyDontIntersect(Vector3[] triangleA, Vector3[] triangleB)
    {
    //    UnityEngine.Debug.Log("INTERSECT CHECK");
        Vector4 planeEquationA = GeometryFunctions.planeEquation(triangleA);
        Vector4 planeEquationB = GeometryFunctions.planeEquation(triangleB);

        bool a01 = pointsOnSameSide(triangleA[0], triangleA[1], planeEquationB);
        bool a02 = pointsOnSameSide(triangleA[0], triangleA[2], planeEquationB);
        if (a01 & a02) return true;
        bool b01 = pointsOnSameSide(triangleB[0], triangleB[1], planeEquationB);
        bool b02 = pointsOnSameSide(triangleB[0], triangleB[2], planeEquationB);
        if (b01 & b02) return true;
        bool a12 = pointsOnSameSide(triangleA[1], triangleA[2], planeEquationB);
        bool b12 = pointsOnSameSide(triangleB[1], triangleB[2], planeEquationB);

        Vector3 D = Vector3.Cross(new Vector3(planeEquationA[0], planeEquationA[1], planeEquationA[2]),
            new Vector3(planeEquationB[0], planeEquationB[1], planeEquationB[2]));

        float pa1, pa2;
        if (a01)            //Its a02 and a12
        {
            pa1 = Vector3.Dot(D, triangleA[0] - triangleA[2]);
            pa2 = Vector3.Dot(D, triangleA[1] - triangleA[2]);
        }
        else if (a02)            //Its a12 and a01
        {
            pa1 = Vector3.Dot(D, triangleA[1] - triangleA[2]);
            pa2 = Vector3.Dot(D, triangleA[0] - triangleA[1]);

        }
        else            //Its a01 and a02
        {
            pa1 = Vector3.Dot(D, triangleA[0] - triangleA[1]);
            pa2 = Vector3.Dot(D, triangleA[0] - triangleA[2]);

        }

        float pb1, pb2;
        if (b01)            //Its b02 and b12
        {
            pb1 = Vector3.Dot(D, triangleB[0] - triangleB[2]);
            pb2 = Vector3.Dot(D, triangleB[1] - triangleB[2]);
        }
        else if (b02)            //Its b12 and b01
        {
            pb1 = Vector3.Dot(D, triangleB[1] - triangleB[2]);
            pb2 = Vector3.Dot(D, triangleB[0] - triangleB[1]);
        }
        else            //Its b01 and b02
        {
            pb1 = Vector3.Dot(D, triangleB[0] - triangleB[1]);
            pb2 = Vector3.Dot(D, triangleB[0] - triangleB[2]);
        }


        UnityEngine.Debug.Log("CHECK FAILED: "+pa1+pa2+pb1+pb2);
        return false;
    }

    //Right now this function assumes that our vertex array is of the form [unique vertices][non-unique vertices]
    //That's not necesarily the case
    public static (Vector3[], int[]) weldVertices(Mesh model)
    {

        Vector3[] vertices = model.vertices;
        int[] triangles = model.triangles;

        //Triangles modification loop
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertexA = vertices[i];
            for (int j = i+1; j < vertices.Length; j++)
            {
                Vector3 vertexB = vertices[j]; 
                if(vertexA == vertexB)
                {
                    //Find all references to j in triangles and set it to be i instead
                    for (int k = 0; k < triangles.Length; k++)
                    {
                        int triIndex = triangles[k];
                        if(triIndex == j)
                        {
                            triangles[k] = i;
                        }
                      //if(triIndex > i)
                      //{
                      //  triangles[k]--;
                    //  }
                    }
                    //Remove vertices[j]
                    //Now j-- because now vertices[j] is a different number
                }
            }
            //Now we can figure out how many vertices we need and construct the new vertex list


        }

        //Now reconstruct vertices
        //For every vertex in vertices
        //If its index doesn't exist in triIndex
        Vector3[] newVertices = new Vector3[triangles.Distinct().Count()];
        foreach (int vertexIndex in triangles.Distinct())
        {
            newVertices[vertexIndex] = model.vertices[vertexIndex];
        }

        //For every pair of equivalent vertices A and B in model's vertex list
        //Update all references to B in the model's triangle's list to be references to A
        //Delete vertex B

        model.triangles = triangles;
        model.vertices = newVertices;
        return (model.vertices, model.triangles);//Dummy line
    }


}
