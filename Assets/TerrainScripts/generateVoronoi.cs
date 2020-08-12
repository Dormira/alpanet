using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateVoronoi : MonoBehaviour
{
    void Start()
    {
        List<Vector2> pointslist = new List<Vector2>();
        pointslist.Add(new Vector2(4, 8));
        pointslist.Add(new Vector2(5, 1));
        pointslist.Add(new Vector2(4, 5));
        pointslist.Add(new Vector2(4, 5));
        pointslist.Add(new Vector2(3, 1));
        pointslist.Add(new Vector2(6, 9));
        generate(pointslist);

        //okaaaaaaaayyyyyyyyyyy generate flat texture, apply it to terrain


    }
    //Implementation of fortune's algorithm
    void generate(List<Vector2> pointEvents)
    {
        //Two kinds of events under fortunes; pointEvents(adding points)
        pointEvents.Sort((p1, p2) => (int)p1.x.CompareTo((int)p2.x));//sort our list of points
        //vertexEvents (removing points)
        //Potential vertexEvents should happen every time the
        List<Vector2> vertexEvents = new List<Vector2>();

        List<Vector2> beachline = new List<Vector2>();//The set of points that defines our beachline
        while (pointEvents.Count > 0)
        {
            //If there are no vertex events, or there are but a point event comes first, then its a point event
            if (vertexEvents.Count == 0 || pointEvents[0][0] < vertexEvents[0][0])
            {
                Vector2 curEvent = pointEvents[0];
                beachline.Add(curEvent);
                pointEvents.RemoveAt(0);
                beachline.Sort((p1, p2) => (int)p1.y.CompareTo((int)p2.y));//Sort top to bottom
            }
            //Otherwise its a vertex event
            else
            {

            }

        }
        



    }



}
