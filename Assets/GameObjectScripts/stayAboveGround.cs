using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stayAboveGround : MonoBehaviour
{
    //This script probably should not exist, figure out a different way of keeping alpacas above ground
    //Because this doesn't work with non-terrain objects

    /*
    void Update()
    {
        
        Vector3 curpos = this.transform.position;
        curpos.y = Terrain.activeTerrain.SampleHeight(curpos);
        if (transform.position[1] < curpos.y)
        {
            curpos.y = curpos.y + 10;
            this.transform.position = curpos;
        }
        
    }
        */
}
