using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class GeometryFunctions{

    public static Vector3[] getNLocationsOnTriangle(Vector3[] triangle, int nPoints)
    {
        Vector3[] points = new Vector3[nPoints];
        
        Vector3 t0 = triangle[0];
        Vector3 t1 = triangle[1];
        Vector3 t2 = triangle[2];

        float seedA, seedB;

        for (int i = 0; i < nPoints; i++)
        {
            seedA = Random.Range(0f, 1f);
            seedB = Random.Range(0f, 1f);

            if(seedA+seedB >= 1){
                seedA = 1 - seedA;
                seedB = 1 - seedB;
            }

            points[i] = new Vector3(t0.x + seedA * (t1.x - t0.x) + seedB * (t2.x - t0.x),
                t0.y + seedA * (t1.y - t0.y) + seedB * (t2.y - t0.y),
                t0.z + seedA * (t1.z - t0.z) + seedB * (t2.z - t0.z));
        }

        return points;
    }

    public static Vector3 getCentroidOfTriangle(Vector3[] triangle)
    {
        Vector3 t0 = triangle[0];
        Vector3 t1 = triangle[1];
        Vector3 t2 = triangle[2];

        Vector3 ret = (t0 + t1 + t2) / 3f;
        return ret;


    }

    /*
     * Calculates and returns the area of the given triangle
     */
    public static float getTriangleArea(Vector3[] triangle)
    {
        Vector3 t0 = triangle[0];
        Vector3 t1 = triangle[1];
        Vector3 t2 = triangle[2];

        float dist01 = Vector3.Distance(t0, t1);
        float dist02 = Vector3.Distance(t0, t2);
        float dist12 = Vector3.Distance(t1, t2);

        float p = (dist01 + dist02 + dist12) / 2;

        return Mathf.Sqrt(Mathf.Abs(p*(p-dist01)*(p-dist02)*p-(dist12)));
    }

    /*
     * Calculates and returns the plane equation of the given triangle
     */
    public static Vector3 getTriangleNormal(Vector3[] triangle)
    {
        //Get the vertices of the triangle
        Vector3 t0 = triangle[0];
        Vector3 t1 = triangle[1];
        Vector3 t2 = triangle[2];

        Vector3 ab = t1 - t0;
        Vector3 ac = t2 - t0;
        //Take the cross product to determine a, b, and c
        Vector3 normal = Vector3.Cross(ab, ac);

        return new Vector3(normal.x, normal.y, normal.z);
    }

    /*
     * Calculates and returns the plane equation of the given triangle
     */
    public static Vector4 planeEquation(Vector3[] triangle){
    //Get the vertices of the triangle
    Vector3 t0 = triangle[0];
    Vector3 t1 = triangle[1];
    Vector3 t2 = triangle[2];

    Vector3 ab = t1 - t0;
    Vector3 ac = t2 - t0;
    //Take the cross product to determine a, b, and c
    Vector3 normal = Vector3.Cross(ab, ac);
    //Plug a, b, and c into the triangle formula to get d
    float d = normal.x * t0.x + normal.y * t0.y + normal.z * t0.z;

    return new Vector4(normal.x, normal.y, normal.z, d);
    }

    /*
     * Calculates whether or not pointA and pointB are on the same side of plane equation ax+by+cz = d
     */
    public static bool pointsOnSameSide(Vector3 pointA, Vector3 pointB, Vector4 planeEquation)
    {
        float a = planeEquation[0];
        float b = planeEquation[1];
        float c = planeEquation[2];
        float d = planeEquation[3];

        //Trivial check for same point
        if (pointA == pointB)
        {
            return true;
        }
        // Otherwise if the signs of the points plugged into the plane equation are the same then they're on the same side
        float res1 = pointA[0] * a + pointA[1] * b + pointA[2] * c + d;
        float res2 = pointB[0] * a + pointB[1] * b + pointB[2] * c + d;
        if (res1 * res2 > 0)
        {
            return true;
        }
        return false;
    }

    /*
     * This is roughly Moller's triangle-triangle intersection algorithm
     * Small modifications to count cases where triangles share one or two vertices as a non-intersection
     * since that's a valid way for triangles to be in a 3Dmodel
     * 
     * Returns True if the triangles don't intersect
     * Returns False if they do
     */
    public static bool triangleNonintersectCheck(Vector3[] triangleA, Vector3[] triangleB)
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

    /*
     * weldVertices should return a vertex and triangle list of an optimized version of the given mesh
     * Welding colocated vertices of our model together leads to cleaner mesh modifications and improves performance somewhat
     * 
     * 
     * I also don't think that this file is the appropriate place for a vertex welding function, need to figure out a better place for it
     * Maybe I should have a separate file just for mesh modification functions
     */

    public static (Vector3[], int[]) weldVertices(Mesh mesh)
    {
        //The shame I feel in using Linq is nothing compared to the frustration I'd feel not using Linq
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);
        
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertexA = vertices[i];
            for (int j = i+1; j < vertices.Count; j++)
            {
                Vector3 vertexB = vertices[j]; 
                if(vertexA == vertexB)
                {
                    //Find all references to j in triangles and set it to be i instead
                    for (int k = 0; k < triangles.Count; k++)
                    {
                        int triIndex = triangles[k];
                        if(triIndex == j)
                        {
                            //Replace if it's a match
                            triangles[k] = i;
                        }
                        if(triIndex > j)
                        {
                            //Or Downshift if it would be affected by our deletion
                            triangles[k] = triangles[k] - 1;
                        }
                    }
                    //Now remove the element at j from vertices because it's a dupe
                    vertices.RemoveAt(j);
                    //And we have to redo that j index because the new j is the former j+1
                    j--;
                }
            }
        }

        mesh.triangles = triangles.ToArray();
        mesh.vertices = vertices.ToArray();
        return (mesh.vertices, mesh.triangles);
    }


}
