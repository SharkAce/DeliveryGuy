using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField]
    private List<WaypointController> nextWaypoints = new List<WaypointController>();

    public WaypointController getNext()
    {
        return nextWaypoints[Random.Range(0, nextWaypoints.Count)];
    }
}
