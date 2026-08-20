using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private DeliveryManager deliveryManager;
    private CarController carInRange;
    private int carCollidersInRange;

    public void Initialize(DeliveryManager manager)
    {
        deliveryManager = manager;
    }

    private void Update()
    {
        if (carInRange != null && Input.GetKeyDown(KeyCode.E))
        {
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
            Debug.Log("Press E to interact.");
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
        }
    }

    private void OnDisable()
    {
        carInRange = null;
        carCollidersInRange = 0;
    }
}