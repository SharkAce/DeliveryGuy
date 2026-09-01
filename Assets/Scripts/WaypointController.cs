using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType
{
    WalkingIntersection,
    DrivingIntersection
}

public class WaypointController : MonoBehaviour
{
    public WaypointType type;
    public List<WaypointController> nextWaypoints = new List<WaypointController>();


    private void OnDrawGizmos()
    {
        switch (type)
        {
            case WaypointType.WalkingIntersection:
                Gizmos.color = Color.blue;
                break;
            case WaypointType.DrivingIntersection:
                Gizmos.color = Color.red;
                break;
        }
        Gizmos.DrawSphere(transform.position, 0.1f);
        
        Gizmos.color = Color.green;
        foreach (var next in nextWaypoints)
        {
            if (next != null)
            {
                DrawArrow(transform.position, next.transform.position, 0.2f);
            }
        }
    }

    private void DrawArrow(Vector3 start, Vector3 dest, float arrowHeadLength)
    {
        Vector3 direction = (dest - start).normalized;
    
        Vector3 directionLeft = Quaternion.AngleAxis(30f, Vector3.back) * direction;
        Vector3 directionRight = Quaternion.AngleAxis(-30f, Vector3.back) * direction;
        
        Gizmos.DrawLine(start, dest);
        
        Gizmos.DrawLine(dest, dest - (directionLeft * arrowHeadLength));
        Gizmos.DrawLine(dest, dest - (directionRight * arrowHeadLength));
    }
}