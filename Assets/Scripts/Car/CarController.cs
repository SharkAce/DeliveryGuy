using UnityEngine;

public class CarController : MonoBehaviour
{
        public struct Controls
    {
        public float driveInput;
        public float brakeInput;
        public float steerInput;
        public bool slideInput;
    }

    [Header("Wheels")]
    public WheelController frontLeft;
    public WheelController frontRight;
    public WheelController rearLeft;
    public WheelController rearRight;

    [Header("Controls")]
    private Controls controls;
    public float maxSteerAngle = 35f;
    public float steerSpeed = 5f;


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
        HandleSteering();
    }

    public void ApplyControls(CarController.Controls new_controls)
    {
        if (new_controls.brakeInput < 0f || new_controls.brakeInput > 1f) return;
        if (new_controls.driveInput < -1f || new_controls.driveInput > 1f) return;
        if (new_controls.steerInput < -1f || new_controls.steerInput > 1f) return;

        controls = new_controls;
    }

    private void FixedUpdate()
    {
        frontLeft.Tick(
            controls.driveInput,
            controls.brakeInput,
            steerAngle,
            false
        );

        frontRight.Tick(
            controls.driveInput,
            controls.brakeInput,
            steerAngle,
            false
        );

        rearLeft.Tick(
            controls.driveInput,
            controls.brakeInput,
            steerAngle,
            controls.slideInput
        );

        rearRight.Tick(
            controls.driveInput,
            controls.brakeInput,
            steerAngle,
            controls.slideInput
        );
    }

    private void HandleSteering()
    {
        float targetAngle = controls.steerInput * maxSteerAngle;

        steerAngle = Mathf.MoveTowards(
            steerAngle,
            targetAngle,
            steerSpeed * Time.deltaTime
        );
    }
}