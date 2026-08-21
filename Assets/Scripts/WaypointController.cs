using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType
{
    Building,
    Parking,
    Intersection
}

public class WaypointController : MonoBehaviour
{
    public WaypointType type;
    public List<WaypointController> nextWaypoints;
}