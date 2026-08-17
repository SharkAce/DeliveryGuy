using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheels")]
    public WheelController frontLeft;
    public WheelController frontRight;
    public WheelController rearLeft;
    public WheelController rearRight;

    [Header("Steering")]
    public float maxSteerAngle = 35f;
    public float steerSpeed = 5f;

    private Rigidbody2D rb;
    private float driveInput;
    private float brakeInput;
    private float steerInput;

    private bool slideInput;
    private float steerAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        frontLeft.Init(rb);
        frontRight.Init(rb);
        rearLeft.Init(rb);
        rearRight.Init(rb);
    }

    void Update()
    {
        driveInput  = Input.GetKey(KeyCode.W) ? 1f : 0f;
        brakeInput  = Input.GetKey(KeyCode.S) ? 1f : 0f;
        
        if (Input.GetKey(KeyCode.A)) steerInput = 1f;
        else if (Input.GetKey(KeyCode.D)) steerInput = -1f;
        else steerInput = 0f;

        slideInput = Input.GetKey(KeyCode.Space);

        HandleSteering();
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