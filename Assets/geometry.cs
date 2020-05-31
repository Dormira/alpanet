using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        if (res1 * res2 > 0)
        {

            return true;
        }
     //   UnityEngine.Debug.Log("POINTS ON OPPOSITE SIDES");
        return false;
    }

    //This function takes THREE POINTS, NOT THREE INDICES
    public static bool trianglesDefinitelyDontIntersect(Vector3[] triangleA, Vector3[] triangleB)
    {

        Vector4 planeEquationA = GeometryFunctions.planeEquation(triangleA);
        Vector4 planeEquationB = GeometryFunctions.planeEquation(triangleB);
      //  UnityEngine.Debug.Log("START FUNC ");
        bool a01 = pointsOnSameSide(triangleA[0], triangleA[1], planeEquationB);
        bool a02 = pointsOnSameSide(triangleA[0], triangleA[2], planeEquationB);
       // UnityEngine.Debug.Log("AS: "+a01+" "+a02);
        if (a01 && a02) {
            return true; 
        }
        bool b01 = pointsOnSameSide(triangleB[0], triangleB[1], planeEquationA);
        bool b02 = pointsOnSameSide(triangleB[0], triangleB[2], planeEquationA);
     //   UnityEngine.Debug.Log("BS: " + b01 + " " + b02);
        if (b01 && b02) { 
            return true; 
        }
     //   UnityEngine.Debug.Log("CHECK FAILED");
        bool a12 = pointsOnSameSide(triangleA[1], triangleA[2], planeEquationB);
        bool b12 = pointsOnSameSide(triangleB[1], triangleB[2], planeEquationB);

        Vector3 D = Vector3.Cross(new Vector3(planeEquationA[0], planeEquationA[1], planeEquationA[2]),
            new Vector3(planeEquationB[0], planeEquationB[1], planeEquationB[2]));

        //projection of vector a onto D is
        //a dot (unit vector b) * (unit vector b)
        //vector a1
        //vector a2
        //     float pa1, pa2;
        Vector3 va1, va2;
        if (a01)            //Its a02 and a12 on the same side
        {
            UnityEngine.Debug.Log()
            va1 = triangleA[0] - triangleA[2];
            va2 = triangleA[2] - triangleA[1];
//
        }
        else if (a02)            //Its a12 and a01
        {

            va1 = triangleA[2] - triangleA[1];
            va2 = triangleA[1] - triangleA[0];
        }
        else            //Its a01 and a02
        {
            va1 = triangleA[1] - triangleA[0];
            va2 = triangleA[0] - triangleA[2];


        }

        Vector3 vb1, vb2;
        if (b01)            //Its b02 and b12
        {
            vb1 = triangleB[0] - triangleB[2];
            vb2 = triangleB[2] - triangleB[1]; 
        }
        else if (b02)            //Its b12 and b01
        {
            
            vb1 = triangleB[2] - triangleB[1];
            vb2 = triangleB[1] - triangleB[0];
        }
        else            //Its b01 and b02
        {
    
            vb1 = triangleB[1] - triangleB[0];
            vb2 = triangleB[0] - triangleB[2];
        }


        float da0 = planeEquationB[0] * va1[0] + planeEquationB[1] * va1[1] + planeEquationB[2] * va1[2] + planeEquationB[3];
        float da1 = planeEquationB[0] * va2[0] + planeEquationB[1] * va2[1] + planeEquationB[2] * va2[2] + planeEquationB[3];
        Vector3 ta1 = va1 + (va2 - va1) * (da0 / (da0 - da1));
        Vector3 ta2 = va2 + (va1 - va2) * (da0 / (da0 - da1));


        float db1 = planeEquationA[0] * vb1[0] + planeEquationA[1] * vb1[1] + planeEquationA[2] * vb1[2] + planeEquationA[3];
        float db2 = planeEquationA[0] * vb2[0] + planeEquationA[1] * vb2[1] + planeEquationA[2] * vb2[2] + planeEquationA[3];
        Vector3 tb1 = vb1 + (vb2 - vb1) * (db1 / (db1 - db2));
        Vector3 tb2 = vb2 + (vb1 - vb2) * (db1 / (db1 - db2));

        if(System.Math.Min(ta1[0], ta2[0]) > System.Math.Max(tb1[0], tb2[0]) ||
           System.Math.Min(tb1[0], tb2[0]) > System.Math.Max(ta1[0], ta2[0]))
        {
            return true;
        }
        if (System.Math.Min(ta1[1], ta2[1]) > System.Math.Max(tb1[1], tb2[1]) ||
   System.Math.Min(tb1[1], tb2[1]) > System.Math.Max(ta1[1], ta2[1]))
        {
            return true;
        }
        if (System.Math.Min(ta1[2], ta2[2]) > System.Math.Max(tb1[2], tb2[2]) ||
   System.Math.Min(tb1[2], tb2[2]) > System.Math.Max(ta1[2], ta2[2]))
        {
            return true;
        }
        UnityEngine.Debug.Log("TA 1 " + ta1);
        UnityEngine.Debug.Log("TA 2 " + ta2);
        UnityEngine.Debug.Log("TB 1 " + tb1);
        UnityEngine.Debug.Log("TB 2 " + tb2);
        //      UnityEngine.Debug.Log("CHECK FAILED: "+pa1+":"+pa2+","+pb1+":"+pb2);
        UnityEngine.Debug.Log(D);
      //  UnityEngine.Debug.Log(Vector3.Dot(D, triangleA[0]) + " " + Vector3.Dot(D, triangleA[1]) + " " + Vector3.Dot(D, triangleA[2]));
    //    UnityEngine.Debug.Log(Vector3.Dot(D, triangleB[0]) + " " + Vector3.Dot(D, triangleB[1]) + " " + Vector3.Dot(D, triangleB[2]));
        UnityEngine.Debug.Log("TRIANGLE A "+triangleA[0]+" "+triangleA[1]+" "+triangleA[2]);
    //    UnityEngine.Debug.Log("PLANE EQUATION A "+planeEquationA[0]+" "+planeEquationA[1]+" "+planeEquationA[2]+" "+planeEquationA[3]);
        UnityEngine.Debug.Log("TRIANGLE B "+triangleB[0] + " " + triangleB[1] + " " + triangleB[2]);
    //    UnityEngine.Debug.Log("PLANE EQUATION B "+planeEquationB[0] + " " + planeEquationB[1] + " " + planeEquationB[2] + " " + planeEquationB[3]);
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
