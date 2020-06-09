using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRandomly : MonoBehaviour
{
    private Vector3 targetLocation;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveToTargetLocation();
    }

//TODO make this time based actually
    void moveToTargetLocation()
    {
        if (this.transform.position[0] == targetLocation[0] && this.transform.position[1] == targetLocation[1])
        {
            Vector3 source_vertex = this.transform.position;

            targetLocation = new Vector3(UnityEngine.Random.Range(source_vertex[0] - 1f, source_vertex[0] + 1f),
                                     UnityEngine.Random.Range(source_vertex[1] - 1f, source_vertex[1] + 1f),
                                     this.transform.position[2]);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(this.transform.position, targetLocation, step);
    }
}
