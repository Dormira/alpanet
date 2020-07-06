using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stayAboveGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curpos = this.transform.position;
        curpos.y = Terrain.activeTerrain.SampleHeight(curpos);
        if (transform.position[1] < curpos.y)
        {
            curpos.y = curpos.y + 5;
            this.transform.position = curpos;
        }
    }
}
