using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System;

public class meshSerializer
{
    //Conversion functions
    static Vector3 serializableVector3ToVector3(serializableVector3 svec3)
    {
        return new Vector3(svec3.x, svec3.y, svec3.z);
    }
    static serializableVector3 vector3ToSerializableVector3(Vector3 vec3)
    {
        serializableVector3 svec3 = new serializableVector3();
        svec3.x = vec3[0];
        svec3.y = vec3[1];
        svec3.z = vec3[2];
        return svec3;
    }
    static Vector4 serializableVector4ToVector4(serializableVector4 svec4)
    {
        return new Vector4(svec4.r, svec4.g, svec4.b, svec4.a);
    }
    static Color serializableVector4ToColor(serializableVector4 svec4)
    {
        //Doesn't like automatically casting vector4 to color for some reason...
        return new Color(svec4.r, svec4.g, svec4.b, svec4.a);
    }
    static serializableVector4 vector4ToSerializableVector4(Vector4 vec4)
    {
        serializableVector4 svec4 = new serializableVector4();
        svec4.r = vec4[0];
        svec4.g = vec4[1];
        svec4.b = vec4[2];
        svec4.a = vec4[3];
        return svec4;
    }

    private static BinaryFormatter formatter = new BinaryFormatter();

    public static void serializeMesh(Mesh mesh, string name)
    {
        //Convert to serializable version of thing that already exists
        serializableMesh smesh = new serializableMesh();
        smesh.vertices = mesh.vertices.Select(x => vector3ToSerializableVector3(x)).ToArray();
        smesh.triangles = mesh.triangles;
        smesh.colors = mesh.colors.Select(x => vector4ToSerializableVector4(x)).ToArray();


        string path = Path.Combine(Application.persistentDataPath, name + ".mesh");
        FileStream stream = new FileStream(path, FileMode.Create);
 
        formatter.Serialize(stream, smesh);
        stream.Close();
    }

    public static Mesh deserializeMesh(string name)
    {
        Mesh mesh;
        string path = Path.Combine(Application.persistentDataPath, name + ".mesh");
        if(File.Exists(path)){
            FileStream stream = new FileStream(path, FileMode.Open);
            serializableMesh smesh = (serializableMesh)formatter.Deserialize(stream);
            stream.Close();

            mesh = new Mesh();
            mesh.vertices = smesh.vertices.Select(x => serializableVector3ToVector3(x)).ToArray();
            mesh.triangles = smesh.triangles;
            mesh.colors = smesh.colors.Select(x => serializableVector4ToColor(x)).ToArray();
        }
        else
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshFilter mf = (MeshFilter)cube.GetComponent("MeshFilter");
            mesh = mf.mesh;
        }

        return mesh;
    }
}

[Serializable]
struct serializableVector3
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

[Serializable]
struct serializableVector4
{
    //Note: I actually have no idea if these correspond to RGBA, but as long as they're in the same order it shouldn't mattter
    public float r;
    public float g;
    public float b;
    public float a;
}

[Serializable]
struct serializableMesh
{
    public serializableVector3[] vertices;
    public int[] triangles;
    public serializableVector4[] colors;
}




