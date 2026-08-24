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

    [Header("Navigation")]
    [SerializeField] private ObjectiveArrow objectiveArrow;

    [Header("UI")]
    [SerializeField] private PhoneUI phoneUI;

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

        if (objectiveArrow != null)
        {
            objectiveArrow.SetTarget(
                CurrentDelivery.PickupPoint.transform
            );
        }

        if (phoneUI != null)
        {
            phoneUI.ShowPickup(
                currentDeliveryIndex + 1,
                deliveries.Length,
                CurrentDelivery
            );
        }
    }

    private void CollectPackage()
    {
        currentState = DeliveryState.CarryingPackage;
        CurrentDelivery.ShowDropOff();

        if (objectiveArrow != null)
        {
            objectiveArrow.SetTarget(
                CurrentDelivery.DropOffPoint.transform
            );
        }

        if (phoneUI != null)
        {
            phoneUI.ShowDropOff(
                currentDeliveryIndex + 1,
                deliveries.Length,
                CurrentDelivery
            );
        }
    }

    private void CompleteCurrentDelivery()
    {
        CurrentDelivery.Hide();
        currentDeliveryIndex++;

        if (currentDeliveryIndex >= deliveries.Length)
        {
            currentState = DeliveryState.Completed;

            if (objectiveArrow != null)
            {
                objectiveArrow.ClearTarget();
            }

            if (phoneUI != null)
            {
                phoneUI.ShowCompleted();
            }

            return;
        }

        BeginCurrentDelivery();
    }
}