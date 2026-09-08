using UnityEngine;

public class PlayerCarControls : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float reversePower = 0.65f;

    [SerializeField] private HUDDisplay hudDisplay;

    private CarController.Controls controls;
    private CarController car;
    private DeliveryManager deliveryManager;
    private Rigidbody2D[] carBodies;
    private bool movementLocked;

    private void Start()
    {
        car = GetComponent<CarController>();
        deliveryManager = FindObjectOfType<DeliveryManager>();
        carBodies = GetComponentsInChildren<Rigidbody2D>();

        controls.driveInput = 0f;
        controls.brakeInput = 0f;
        controls.steerInput = 0f;
        controls.slideInput = false;
    }

    private void Update()
    {
        movementLocked =
            deliveryManager != null &&
            deliveryManager.IsDialogueActive;

        if (movementLocked)
        {
            controls.driveInput = 0f;
            controls.brakeInput = 1f;
            controls.steerInput = 0f;
            controls.slideInput = false;

            car.ApplyControls(controls);

            if (hudDisplay != null)
            {
                hudDisplay.ShowDialogueHint();
            }

            return;
        }

        if (hudDisplay != null)
        {
            hudDisplay.HideDialogueHint();
        }

        controls.brakeInput = 0f;

        if (Input.GetKey(KeyCode.W))
            controls.driveInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            controls.driveInput = -reversePower;
        else
            controls.driveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            controls.steerInput = 1f;
        else if (Input.GetKey(KeyCode.D))
            controls.steerInput = -1f;
        else
            controls.steerInput = 0f;

        controls.slideInput = Input.GetKey(KeyCode.Space);

        car.ApplyControls(controls);
    }

    private void FixedUpdate()
    {
        if (!movementLocked)
        {
            return;
        }

        foreach (Rigidbody2D body in carBodies)
        {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
        }
    }
}