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
        Vector3 N2 = Vector3.Cross(triangleB[1] - triangleB[0], triangleB[2] - triangleB[0]);
        N2.Normalize();
        float d2 = Vector3.Dot(N2 * (-1), triangleB[0]);

        float da0 = Vector3.Dot(N2, triangleA[0]) + d2;
        float da1 = Vector3.Dot(N2, triangleA[1]) + d2;
        float da2 = Vector3.Dot(N2, triangleA[2]) + d2;
        if(da0 != 0 && da1 != 0 && da2 != 0 && da0 > 0 && da1 > 0 && da2 > 0)
        {
            return true;
        }
        if (da0 != 0 && da1 != 0 && da2 != 0 && da0 < 0 && da1 < 0 && da2 < 0)
        {
            return true;
        }
        Vector3 N1 = Vector3.Cross(triangleA[1] - triangleA[0], triangleA[2] - triangleA[0]);
        N1.Normalize();
        float d1 = Vector3.Dot(N1 * (-1), triangleA[0]);

        float db0 = Vector3.Dot(N1, triangleB[0]) + d1;
        float db1 = Vector3.Dot(N1, triangleB[1]) + d1;
        float db2 = Vector3.Dot(N1, triangleB[2]) + d1;
        if (db0 != 0 && db1 != 0 && db2 != 0 && db0 > 0 && db1 > 0 && db2 > 0)
        {
            return true;
        }
        if (db0 != 0 && db1 != 0 && db2 != 0 && db0 < 0 && db1 < 0 && db2 < 0)
        {
            return true;
        }
        //coplanarity check to go here, unlikely tho can probs just skip it for now
        //one point contact checks
        if((da0 == 0 && da1 > 0 && da2 > 0 ) || (da0 == 0 && da1 < 0 && da2 < 0))
        {
            return true;
        }
        if ((da0 > 0 && da1 == 0 && da2 > 0) || (da0 < 0 && da1 == 0 && da2 < 0))
        {
            return true;
        }
        if ((da0 > 0 && da1 > 0 && da2 == 0) || (da0 < 0 && da1 < 0 && da2 == 0))
        {
            return true;
        }
        if ((db0 == 0 && db1 > 0 && db2 > 0) || (db0 == 0 && db1 < 0 && db2 < 0))
        {
            return true;
        }
        if ((db0 > 0 && db1 == 0 && db2 > 0) || (db0 < 0 && db1 == 0 && db2 < 0))
        {
            return true;
        }
        if ((db0 > 0 && db1 > 0 && db2 == 0) || (db0 < 0 && db1 < 0 && db2 == 0))
        {
            return true;
        }
        //two point contact checks
        if ((da0 == 0 && da1 == 0 && da2 > 0) || (da0 == 0 && da1 == 0 && da2 < 0))
        {
            return true;
        }
        if ((da0 > 0 && da1 == 0 && da2 == 0) || (da0 < 0 && da1 == 0 && da2 == 0))
        {
            return true;
        }
        if ((da0 == 0 && da1 > 0 && da2 == 0) || (da0 == 0 && da1 < 0 && da2 == 0))
        {
            return true;
        }
        if ((db0 == 0 && db1 == 0 && db2 > 0) || (db0 == 0 && db1 == 0 && db2 < 0))
        {
            return true;
        }
        if ((db0 > 0 && db1 == 0 && db2 == 0) || (db0 < 0 && db1 == 0 && db2 == 0))
        {
            return true;
        }
        if ((db0 == 0 && db1 > 0 && db2 == 0) || (db0 == 0 && db1 < 0 && db2 == 0))
        {
            return true;
        }

        Vector3 D = Vector3.Cross(N1,N2);
        D.Normalize();
        
        float dv0=9001, dv1= 9001, dv2= 9001;

        float ta1, ta2;
        float pa0= 9001, pa1= 9001, pa2= 9001;
        if ((da0 > 0 && da1 <= 0 && da2 <= 0) || (da0 < 0 && da1 >= 0 && da2 >= 0))
        {
            pa0 = Vector3.Dot(D, triangleA[1]);
            pa1 = Vector3.Dot(D, triangleA[0]);
            pa2 = Vector3.Dot(D, triangleA[2]);
            dv0 = da1;
            dv1 = da0;
            dv2 = da2;
        }
        else if ((da0 <= 0 && da1 > 0 && da2 <= 0) || (da0 >= 0 && da1 < 0 && da2 >= 0))
        {
            pa0 = Vector3.Dot(D, triangleA[0]);
            pa1 = Vector3.Dot(D, triangleA[1]);
            pa2 = Vector3.Dot(D, triangleA[2]);
            dv0 = da0;
            dv1 = da1;
            dv2 = da2;
        }
        else if ((da0 <= 0 && da1 <= 0 && da2 > 0) || (da0 >= 0 && da1 >= 0 && da2 < 0))
        {
            pa0 = Vector3.Dot(D, triangleA[0]);
            pa1 = Vector3.Dot(D, triangleA[2]);
            pa2 = Vector3.Dot(D, triangleA[1]);
            dv0 = da0;
            dv1 = da2;
            dv2 = da1;
        }
        ta1 = pa0 + (pa1 - pa0) * (dv0 / (dv0 - dv1));
        ta2 = pa1 + (pa2 - pa1) * (dv1 / (dv1 - dv2));


        float tb1, tb2;
        float pb0 = 9001, pb1 = 9001, pb2 = 9001;
        if ((db0 > 0 && db1 <= 0 && db2 <= 0) || (db0 < 0 && db1 >= 0 && db2 >= 0))
        {
            pb0 = Vector3.Dot(D, triangleB[1]);
            pb1 = Vector3.Dot(D, triangleB[0]);
            pb2 = Vector3.Dot(D, triangleB[2]);
            dv0 = db1;
            dv1 = db0;
            dv2 = db2;
        }
        else if ((db0 <= 0 && db1 > 0 && db2 <= 0) || (db0 >= 0 && db1 < 0 && db2 >= 0))
        {
            pb0 = Vector3.Dot(D, triangleB[0]);
            pb1 = Vector3.Dot(D, triangleB[1]);
            pb2 = Vector3.Dot(D, triangleB[2]);
            dv0 = db0;
            dv1 = db1;
            dv2 = db2;
        }
        else if ((db0 <= 0 && db1 <= 0 && db2 > 0) || (db0 >= 0 && db1 >= 0 && db2 < 0))
        {
            pb0 = Vector3.Dot(D, triangleB[0]);
            pb1 = Vector3.Dot(D, triangleB[2]);
            pb2 = Vector3.Dot(D, triangleB[1]);
            dv0 = db0;
            dv1 = db2;
            dv2 = db1;
        }
        tb1 = pb0 + (pb1 - pb0) * (dv0 / (dv0 - dv1));
        tb2 = pb1 + (pb2 - pb1) * (dv1 / (dv1 - dv2));
        if(System.Math.Min(tb1, tb2)+0.1 > System.Math.Max(ta1, ta2))
        {
            return true;
        }
        if (System.Math.Min(ta1, ta2)+0.1 > System.Math.Max(tb1, tb2))
        {
            return true;
        }

        return false;
    }

    //Right now this function assumes that our vertex array is of the form [unique vertices][non-unique vertices]
    //That's not necesarily the case
    public static (Vector3[], int[]) weldVertices(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

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
                    }
                }
            }
        }

        //Now reconstruct vertices
        //For every vertex in vertices
        //If its index doesn't exist in triIndex
        Vector3[] newVertices = new Vector3[triangles.Distinct().Count()];
      //  int nvi = 0;
        foreach (int vertexIndex in triangles.Distinct())
        {
        //    UnityEngine.Debug.Log(vertexIndex + "/" + triangles.Distinct());
            UnityEngine.Debug.Log(vertexIndex+"/"+newVertices.Length);
            UnityEngine.Debug.Log(vertexIndex + "/" + mesh.vertices.Length);
            newVertices[vertexIndex] = mesh.vertices[vertexIndex];
        }

        //For every pair of equivalent vertices A and B in model's vertex list
        //Update all references to B in the model's triangle's list to be references to A
        //Delete vertex B

        mesh.triangles = triangles;
        mesh.vertices = newVertices;
        return (mesh.vertices, mesh.triangles);//Dummy line
    }


}
