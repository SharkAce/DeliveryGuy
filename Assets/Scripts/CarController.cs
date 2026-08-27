using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheels")]
    public WheelController frontLeft;
    public WheelController frontRight;
    public WheelController rearLeft;
    public WheelController rearRight;

    [Header("Controls")]
    public float maxSteerAngle = 35f;
    public float steerSpeed = 5f;
    public float maxAISpeed = 10f;
    private Rigidbody2D rb;
    private float driveInput;
    private float brakeInput;
    private float steerInput;

    private bool slideInput;
    private float steerAngle = 0f;
    public bool playerControlled = false;
    [SerializeField] private WaypointController currentWaypoint = null;
    [SerializeField] private WaypointController previousWaypoint = null;
    [SerializeField] private float waypointTriggerDist = 1f;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        frontLeft.Init(rb);
        frontRight.Init(rb);
        rearLeft.Init(rb);
        rearRight.Init(rb);
    }

    public void InitAI(WaypointController startWp, WaypointController previousWp)
    {
        if (startWp == null || previousWp == null) return;
        currentWaypoint = startWp;
        previousWaypoint = previousWp;

        // Select a random point between the two waypoints
        transform.position = Vector3.Lerp(startWp.transform.position, previousWp.transform.position, Random.value);
    }

    void Update()
    {
        currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;

        if (playerControlled)
        {
            ApplyKeyboardControls();
        }
        else
        {
            ApplyAIControls();
        }

        HandleSteering();
    }

    void ApplyKeyboardControls()
    {
        driveInput  = Input.GetKey(KeyCode.W) ? 1f : 0f;
        brakeInput  = Input.GetKey(KeyCode.S) ? 1f : 0f;
        
        if (Input.GetKey(KeyCode.A)) steerInput = 1f;
        else if (Input.GetKey(KeyCode.D)) steerInput = -1f;
        else steerInput = 0f;

        slideInput = Input.GetKey(KeyCode.Space);
    }

    void ApplyAIControls()
    {
        Vector3 directionToWaypoint = (currentWaypoint.transform.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.up, directionToWaypoint);
        float cross = Vector3.Cross(transform.up, directionToWaypoint).z;

        float targetSpeed = maxAISpeed * dot;

        driveInput = currentSpeed < targetSpeed ? 1f : 0f;
        brakeInput = currentSpeed > targetSpeed ? 0f : 1f;
        steerInput = cross;

        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) < waypointTriggerDist) 
            SelectAINextWaypoint();
    }

    void SelectAINextWaypoint()
    {
        foreach (WaypointController wp in currentWaypoint.nextWaypoints)
        {
            if (wp != currentWaypoint && wp != previousWaypoint)
            {
                previousWaypoint = currentWaypoint;
                currentWaypoint = wp;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        // Only apply sliding to the back wheels
        frontLeft.Tick(driveInput,  brakeInput, steerAngle, false);
        frontRight.Tick(driveInput, brakeInput, steerAngle, false);
        rearLeft.Tick(driveInput,   brakeInput, steerAngle, slideInput);
        rearRight.Tick(driveInput,  brakeInput, steerAngle, slideInput);
    }

    void HandleSteering()
    {
        float targetAngle = steerInput * maxSteerAngle;
        steerAngle = Mathf.MoveTowards(steerAngle, targetAngle, steerSpeed * Time.deltaTime);
    }
}