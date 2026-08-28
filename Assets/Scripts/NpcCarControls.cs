using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarControls : MonoBehaviour
{
    [SerializeField] private float maxAISpeed = 10f;
    [SerializeField] private WaypointController currentWaypoint = null;
    [SerializeField] private float waypointTriggerDist = 1f;
    private float currentSpeed = 0;
    private CarController.Controls controls = new CarController.Controls();
    private CarController car;

    void Start()
    {
        car = GetComponent<CarController>();
        controls.driveInput = 0f;
        controls.brakeInput = 0f;
        controls.steerInput = 0f;
        controls.slideInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
        ApplyControls();
    }
    public void Init(WaypointController startWp, WaypointController previousWp)
    {
        if (startWp == null || previousWp == null) return;
        currentWaypoint = startWp;

        // Select a random point between the two waypoints
        transform.position = Vector3.Lerp(startWp.transform.position, previousWp.transform.position, Random.value);
        Vector2 directionToWaypoint = (startWp.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToWaypoint.y, directionToWaypoint.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void ApplyControls()
    {
        Vector3 directionToWaypoint = (currentWaypoint.transform.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.up, directionToWaypoint);
        float cross = Vector3.Cross(transform.up, directionToWaypoint).z;

        float targetSpeed = maxAISpeed * dot;

        controls.driveInput = currentSpeed < targetSpeed ? 1f : 0f;
        controls.brakeInput = currentSpeed > targetSpeed ? 1f : 0f;
        controls.steerInput = Mathf.Clamp(cross, -1f, 1f);

        car.ApplyControls(controls);

        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) < waypointTriggerDist) 
            SelectNextWaypoint();
    }

    void SelectNextWaypoint()
    {
        currentWaypoint = currentWaypoint.nextWaypoints[Random.Range(0, currentWaypoint.nextWaypoints.Count)];
    }
}
