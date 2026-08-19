using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private DeliveryManager deliveryManager;

    public void Initialize(DeliveryManager manager)
    {
        deliveryManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CarController car = other.GetComponentInParent<CarController>();

        if (car == null || deliveryManager == null)
        {
            return;
        }

        deliveryManager.ReachPoint(this);
    }
}