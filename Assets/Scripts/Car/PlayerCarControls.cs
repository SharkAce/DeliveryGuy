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
    private bool previousMovementLocked;

    private void Start()
    {
        car = GetComponent<CarController>();
        deliveryManager = FindObjectOfType<DeliveryManager>();
        carBodies = GetComponentsInChildren<Rigidbody2D>();

        /* Auto assign HUD*/
        if (hudDisplay == null) hudDisplay = FindObjectOfType<HUDDisplay>();

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

        if (movementLocked != previousMovementLocked)
        {
            SetCarPhysicsEnabled(!movementLocked);
            previousMovementLocked = movementLocked;
        }

        if (movementLocked)
        {
            controls.driveInput = 0f;
            controls.brakeInput = 1f;
            controls.steerInput = 0f;
            controls.slideInput = false;

            car.ApplyControls(controls);

            bool leftClicked = Input.GetMouseButtonDown(0);

            bool otherMouseClicked =
                Input.GetMouseButtonDown(1) ||
                Input.GetMouseButtonDown(2);

            bool pressedInvalidKey =
                Input.anyKeyDown &&
                !leftClicked &&
                !otherMouseClicked &&
                !Input.GetKeyDown(KeyCode.Return) &&
                !Input.GetKeyDown(KeyCode.KeypadEnter) &&
                !Input.GetKeyDown(KeyCode.Escape);

            if (hudDisplay != null &&
                (pressedInvalidKey || otherMouseClicked))
            {
                hudDisplay.ShowDialogueHint();
            }

            return;
        }

        controls.brakeInput = 0f;

        bool accelerate = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        bool reverse =
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow);

        bool steerLeft =
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow);

        bool steerRight =
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);

        if (accelerate)
        {
            controls.driveInput = 1f;
        }
        else if (reverse)
        {
            controls.driveInput = -reversePower;
        }
        else
        {
            controls.driveInput = 0f;
        }

        if (steerLeft)
        {
            controls.steerInput = 1f;
        }
        else if (steerRight)
        {
            controls.steerInput = -1f;
        }
        else
        {
            controls.steerInput = 0f;
        }

        controls.slideInput = Input.GetKey(KeyCode.Space);

        car.ApplyControls(controls);
    }

    private void SetCarPhysicsEnabled(bool isEnabled)
    {
        if (carBodies == null)
        {
            return;
        }

        foreach (Rigidbody2D body in carBodies)
        {
            if (body == null)
            {
                continue;
            }

            if (!isEnabled)
            {
                body.velocity = Vector2.zero;
                body.angularVelocity = 0f;
            }

            body.simulated = isEnabled;
        }
    }

    private void OnDisable()
    {
        /* Never leave the car outside physics if this component is disabled. */
        SetCarPhysicsEnabled(true);
    }
}
