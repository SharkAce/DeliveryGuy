using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransportMode
{
    Walking,
    Driving
}


public class NpcController : MonoBehaviour
{
    [SerializeField] private CarController car;
    [SerializeField] private WaypointController home;
    [SerializeField] private WaypointController destination;
    [SerializeField] private Queue<WaypointController> subDestinations = new Queue<WaypointController>();
    [SerializeField] private TransportMode transportMode;
    // Start is called before the first frame update
    void Start()
    {
        if (subDestinations.Count == 0) GeneratePath();
    }

    // Update is called once per frame
    void Update()
    {
        if (subDestinations.Count == 0) return;

        Vector3 waypointPos = subDestinations.Peek().transform.position;

        if (Vector3.Distance(transform.position, waypointPos) < 0.01)
        {
            subDestinations.Dequeue();
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            subDestinations.Peek().transform.position,
            2 * Time.deltaTime
        );
    }

    void GeneratePath()
    {
        WaypointController current = home;
        while(current != destination)
        {
            Debug.Log(current.nextWaypoints);
            subDestinations.Enqueue(current.nextWaypoints[0]);
            current = current.nextWaypoints[0];
        }
    }
}
