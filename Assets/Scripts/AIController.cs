using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public WaypointController currentWaypoint;
    public bool goToNext = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            currentWaypoint.transform.position.x, currentWaypoint.transform.position.y, transform.position.z);

        if (goToNext)
        {
            currentWaypoint = currentWaypoint.getNext();
            goToNext = false;
        }
    }
}