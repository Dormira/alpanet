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
    /*
     * This code takes a mesh and serializes it for loading later
     * I wrote this because Unity does not allow for direct serialization of meshes,
     * but I still need to save them to disk as part of an in progress save/load system
     * 
     * This code was written before I moved to using unity's skinned mesh renderer class for the alpacas
     * And so this code won't work for the current iteration of the alpaca models, which contain bones and colliders and such 
     * TODO: Add skinned mesh renderer serialization code
     */
    
    //This formatter is for converting our serializable classes to binary
    private static BinaryFormatter formatter = new BinaryFormatter();

    /*
     * Conversion functions
     */
    static Vector3 serializableVector3ToVector3(SerializableVector3 svec3)
    {
        //Take a serializable vector3 and return a normal vector3
        return new Vector3(svec3.x, svec3.y, svec3.z);
    }
    static SerializableVector3 vector3ToSerializableVector3(Vector3 vec3)
    {
        //Take a regular vector3 and return a serializable vector3
        SerializableVector3 svec3 = new SerializableVector3(vec3[0], vec3[1], vec3[2]);
        return svec3;
    }
    static Vector4 serializableVector4ToVector4(SerializableVector4 svec4)
    {
        //Take a serializable vector4 and convert it to a standard vector4
        return new Vector4(svec4.w, svec4.x, svec4.y, svec4.z);
    }
    static Color serializableVector4ToColor(SerializableVector4 svec4)
    {
        //Take a serializable vector4 and convert it into a color object
        return new Color(svec4.w, svec4.x, svec4.y, svec4.z);
    }
    static SerializableVector4 vector4ToSerializableVector4(Vector4 vec4)
    {
        //Take a vector4 and convert it into its serializable version
        SerializableVector4 svec4 = new SerializableVector4(vec4.w, vec4.x, vec4.y, vec4.z);
        return svec4;
    }

    public static void serializeMesh(Mesh mesh, string name)
    {
        //Take a mesh and a filename and save it to disk
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
        /*
         * Takes a filename and deserializes the associated mesh
         * Returns the default cube mesh if there's no mesh saved to disk
         */
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
            //Load a placeholder cube so that we can at least fail gracefully
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshFilter mf = (MeshFilter)cube.GetComponent("MeshFilter");
            mesh = mf.mesh;
        }

        return mesh;
    }
}

[Serializable]
struct SerializableVector3
{
    //This is a serializable version of Unity's vector3 class
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SerializableVector3(Vector3 vec3)
    {
        this.x = vec3.x;
        this.y = vec3.y;
        this.z = vec3.z;
    }
}

[Serializable]
struct SerializableVector4
{
    //This is a serializable version of Unity's vector4 class
    //Note: I actually don't know if these correspond to RGBA, but as long as they're in the same order when we serialize/deserialize it shouldn't mattter
    public float w { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public SerializableVector4(float w, float x, float y, float z)
    {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SerializableVector4(Vector4 vec4)
    {
        this.w = vec4.w;
        this.x = vec4.x;
        this.y = vec4.y;
        this.z = vec4.z;
    }
}

[Serializable]
struct serializableMesh
{
    /*
     * This is a serializable version of Unity's mesh class
     * It contains the minimum amount of information necessary to construct a mesh
     */
    public SerializableVector3[] vertices;
    public int[] triangles;
    public SerializableVector4[] colors;
}

[Serializable]
struct SerializableSkinnedMesh
{
    //NYI
}




