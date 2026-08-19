using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    private enum DeliveryState
    {
        WaitingForPickup,
        CarryingPackage,
        Completed
    }

    [Header("Deliveries in Order")]
    [SerializeField] private DeliveryRoute[] deliveries;

    private int currentDeliveryIndex;
    private DeliveryState currentState;

    private DeliveryRoute CurrentDelivery
    {
        get { return deliveries[currentDeliveryIndex]; }
    }

    private void Start()
    {
        if (deliveries == null || deliveries.Length == 0)
        {
            Debug.LogError("No deliveries have been assigned.");
            return;
        }

        for (int i = 0; i < deliveries.Length; i++)
        {
            deliveries[i].Initialize(this);
            deliveries[i].Hide();
        }

        currentDeliveryIndex = 0;
        BeginCurrentDelivery();
    }

    public void ReachPoint(DeliveryPoint reachedPoint)
    {
        if (currentState == DeliveryState.WaitingForPickup &&
            reachedPoint == CurrentDelivery.PickupPoint)
        {
            CollectPackage();
        }
        else if (currentState == DeliveryState.CarryingPackage &&
                 reachedPoint == CurrentDelivery.DropOffPoint)
        {
            CompleteCurrentDelivery();
        }
    }

    private void BeginCurrentDelivery()
    {
        currentState = DeliveryState.WaitingForPickup;
        CurrentDelivery.ShowPickup();

        Debug.Log(
            "Delivery " + (currentDeliveryIndex + 1) +
            ": collect the package."
        );
    }

    private void CollectPackage()
    {
        currentState = DeliveryState.CarryingPackage;
        CurrentDelivery.ShowDropOff();

        Debug.Log("Package collected! Drive to the delivery point.");
    }

    private void CompleteCurrentDelivery()
    {
        CurrentDelivery.Hide();
        currentDeliveryIndex++;

        if (currentDeliveryIndex >= deliveries.Length)
        {
            currentState = DeliveryState.Completed;
            Debug.Log("All deliveries completed!");
            return;
        }

        Debug.Log("Delivery completed!");
        BeginCurrentDelivery();
    }
}