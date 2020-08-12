using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorChanger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("up"))
        {
            changeColors();
        }
    }

    public void changeColors()
    {
        Mesh oldmesh = null;

        //Figure out a better way than this thing yikes
        if (GetComponent<MeshFilter>())
        {
            oldmesh = GetComponent<MeshFilter>().sharedMesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            oldmesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
        int numverts = oldmesh.vertexCount;

        Color colorStart = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            1f);
        Color colorEnd = new Color(UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
           1f);

        Color[] colors = new Color[numverts];

        for (int i = 0; i < numverts; i++)
            colors[i] = Color.Lerp(colorStart, colorEnd, (float)i/(float)numverts);


        oldmesh.colors = colors;
    }
}
