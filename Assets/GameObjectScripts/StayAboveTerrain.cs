using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAboveTerrain : MonoBehaviour
{
    //This script keeps the object its attached to above whatever terrain it's on
    //It doesn't work for non-terrain objects, and so is not particularly useful for the round world
    
    void Start()
    {
        //If there's no terrain then disable this script
        if(Terrain.activeTerrain == null){
            Destroy(this);
        }
    }

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
        
}
