using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRandomly : MonoBehaviour
{
    private Vector3 targetLocation;
    public float speed = 0.5f;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        Vector3 source_vertex = this.transform.position;
        targetLocation = new Vector3(UnityEngine.Random.Range(source_vertex[0] - 1f, source_vertex[0] + 1f),
                                     source_vertex[1],
                                     UnityEngine.Random.Range(source_vertex[2] - 1f, source_vertex[2] + 1f));
    }

    // Update is called once per frame
    void Update()
    {
        moveToTargetLocation();
        Vector3 curpos = this.transform.position;
        curpos.y = Terrain.activeTerrain.SampleHeight(curpos);
        if(transform.position[1]+5 < curpos.y)
        {
            this.transform.position = curpos;
        }

    }

    void moveToTargetLocation()
    {
        float timeDiff = Time.time - startTime;
        if (timeDiff > 2 || (this.transform.position[0] == targetLocation[0] && this.transform.position[2] == targetLocation[2]))
        {
            startTime = Time.time;
            Vector3 source_vertex = this.transform.position;

            targetLocation = new Vector3(UnityEngine.Random.Range(source_vertex[0] - 10f, source_vertex[0] + 10f),
                                     source_vertex[1], 
                                     UnityEngine.Random.Range(source_vertex[2] - 10f, source_vertex[2] + 10f));
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(this.transform.position, targetLocation, step);
    }
}
