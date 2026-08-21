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

    [Header("Movement")]
    [Range(0.1f, 1f)]
    public float reversePower = 0.65f;

    private Rigidbody2D rb;
    private float driveInput;
    private float brakeInput;
    private float steerInput;

    private bool slideInput;
    private float steerAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        frontLeft.Init(rb);
        frontRight.Init(rb);
        rearLeft.Init(rb);
        rearRight.Init(rb);
    }

    private void Update()
    {
        HandleMovementInput();
        HandleSteeringInput();

        slideInput = Input.GetKey(KeyCode.Space);

        HandleSteering();
    }

    private void FixedUpdate()
    {
        frontLeft.Tick(
            driveInput,
            brakeInput,
            steerAngle,
            false
        );

        frontRight.Tick(
            driveInput,
            brakeInput,
            steerAngle,
            false
        );

        rearLeft.Tick(
            driveInput,
            brakeInput,
            steerAngle,
            slideInput
        );

        rearRight.Tick(
            driveInput,
            brakeInput,
            steerAngle,
            slideInput
        );
    }

    private void HandleMovementInput()
    {
        brakeInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            driveInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            driveInput = -reversePower;
        }
        else
        {
            driveInput = 0f;
        }
    }

    private void HandleSteeringInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            steerInput = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steerInput = -1f;
        }
        else
        {
            steerInput = 0f;
        }
    }

    private void HandleSteering()
    {
        float targetAngle = steerInput * maxSteerAngle;

        steerAngle = Mathf.MoveTowards(
            steerAngle,
            targetAngle,
            steerSpeed * Time.deltaTime
        );
    }
}