using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarControls : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private WaypointController currentWaypoint = null;
    [SerializeField] private float waypointTriggerDist = 1f;
    [SerializeField] private float detectionDistance = 5f;
    private float currentSpeed = 0;
    private CarController.Controls controls = new CarController.Controls();
    private CarController car;
    private float collisionRayOffset;
    [SerializeField] private float collisionRaySpeedFactor = 0.5f;

    void Start()
    {
        car = GetComponent<CarController>();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        collisionRayOffset = boxCollider.size.y / 2;
        
        controls.driveInput = 0f;
        controls.brakeInput = 0f;
        controls.steerInput = 0f;
        controls.slideInput = false;
    }

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

        float targetSpeed = maxSpeed * dot;

        // Slowdown if a car is in front
        Vector3 raycastStart = transform.position + transform.up * collisionRayOffset;
        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastStart, transform.up, detectionDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.GetComponent<CarController>() != null)
            {
                targetSpeed *= collisionRaySpeedFactor;
                break;
            }
        }

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
