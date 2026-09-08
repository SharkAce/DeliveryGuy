using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private DeliveryManager deliveryManager;
    private CarController carInRange;
    private int carCollidersInRange;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;

    private void Awake()
    {
        SetInteractionPrompt(false);
    }

    public void Initialize(DeliveryManager manager)
    {
        deliveryManager = manager;
    }

    private void Update()
    {
        if (carInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            SetInteractionPrompt(false);
            deliveryManager.ReachPoint(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CarController car = other.GetComponentInParent<CarController>();

        if (car == null || deliveryManager == null)
        {
            return;
        }

        if (carInRange == null)
        {
            carInRange = car;
            carCollidersInRange = 1;

            SetInteractionPrompt(true);
        }
        else if (car == carInRange)
        {
            carCollidersInRange++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CarController car = other.GetComponentInParent<CarController>();

        if (car == null || car != carInRange)
        {
            return;
        }

        carCollidersInRange = Mathf.Max(0, carCollidersInRange - 1);

        if (carCollidersInRange == 0)
        {
            carInRange = null;
            SetInteractionPrompt(false);
        }
    }

    private void OnDisable()
    {
        carInRange = null;
        carCollidersInRange = 0;
        SetInteractionPrompt(false);
    }

    private void SetInteractionPrompt(bool visible)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(visible);
        }
    }
}