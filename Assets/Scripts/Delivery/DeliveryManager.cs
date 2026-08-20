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

    [Header("Minimap")]
    [SerializeField]
    private MinimapDeliveryMarkers minimapMarkers;

    [Header("UI")]
    [SerializeField] private PhoneUI phoneUI;

    [Header("Food Quality")]
    [SerializeField] private float startingFoodQuality = 100f;
    [SerializeField] private float penaltyPerImpactSpeed = 2f;
    [SerializeField] private float minimumCollisionPenalty = 2f;
    [SerializeField] private float maximumCollisionPenalty = 20f;

    private int currentDeliveryIndex;
    private DeliveryState currentState;

    private float deliveryElapsedTime;
    private bool timerRunning;
    private float currentFoodQuality;

    public int CurrentDeliveryNumber
    {
        get
        {
            if (deliveries == null || deliveries.Length == 0)
            {
                return 0;
            }

            return Mathf.Min(
                currentDeliveryIndex + 1,
                deliveries.Length
            );
        }
    }

    public int CompletedDeliveries
    {
        get { return currentDeliveryIndex; }
    }

    public int TotalDeliveries
    {
        get
        {
            return deliveries == null ? 0 : deliveries.Length;
        }
    }

    public bool IsCarryingPackage
    {
        get
        {
            return currentState == DeliveryState.CarryingPackage;
        }
    }

    public bool AllDeliveriesCompleted
    {
        get
        {
            return currentState == DeliveryState.Completed;
        }
    }

    public float DeliveryElapsedTime
    {
        get { return deliveryElapsedTime; }
    }

    public float TargetDeliveryTime
    {
        get
        {
            if (AllDeliveriesCompleted || deliveries == null)
            {
                return 0f;
            }

            return CurrentDelivery.TargetDeliveryTime;
        }
    }

    public float RemainingDeliveryTime
    {
        get
        {
            return Mathf.Max(
                0f,
                TargetDeliveryTime - deliveryElapsedTime
            );
        }
    }

    public bool IsTimerRunning
    {
        get { return timerRunning; }
    }

    public float CurrentFoodQuality
    {
        get { return currentFoodQuality; }
    }

    private DeliveryRoute CurrentDelivery
    {
        get { return deliveries[currentDeliveryIndex]; }
    }

    private void Start()
    {
        if (deliveries == null || deliveries.Length == 0)
        {
            Debug.LogError("No deliveries have been assigned.");
            enabled = false;
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

    private void Update()
    {
        if (timerRunning)
        {
            deliveryElapsedTime += Time.deltaTime;
        }
    }

    public void ReachPoint(DeliveryPoint reachedPoint)
    {
        if (currentState == DeliveryState.WaitingForPickup &&
            reachedPoint == CurrentDelivery.PickupPoint)
        {
            CollectPackage();
        }
        else if (
            currentState == DeliveryState.CarryingPackage &&
            reachedPoint == CurrentDelivery.DropOffPoint
        )
        {
            CompleteCurrentDelivery();
        }
    }

    private void BeginCurrentDelivery()
    {
        currentState = DeliveryState.WaitingForPickup;
        deliveryElapsedTime = 0f;
        timerRunning = false;

        currentFoodQuality = Mathf.Clamp(
            startingFoodQuality,
            0f,
            100f
        );

        CurrentDelivery.ShowPickup();

        if (minimapMarkers != null)
        {
            minimapMarkers.ShowPickup(currentDeliveryIndex);
        }

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
        deliveryElapsedTime = 0f;
        timerRunning = CurrentDelivery.IsTimedDelivery;

        CurrentDelivery.ShowDropOff();

        if (minimapMarkers != null)
        {
            minimapMarkers.ShowDropOff(currentDeliveryIndex);
        }

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
                CurrentDelivery,
                currentFoodQuality
            );
        }
    }

    private void CompleteCurrentDelivery()
    {
        timerRunning = false;

        float completionTime = deliveryElapsedTime;
        float completionQuality = currentFoodQuality;
        bool wasTimed = CurrentDelivery.IsTimedDelivery;

        CurrentDelivery.Hide();
        currentDeliveryIndex++;

        if (currentDeliveryIndex >= deliveries.Length)
        {
            currentState = DeliveryState.Completed;

            if (objectiveArrow != null)
            {
                objectiveArrow.ClearTarget();
            }

            if (minimapMarkers != null)
            {
                minimapMarkers.HideAll();
            }

            if (phoneUI != null)
            {
                phoneUI.ShowCompleted(completionTime, completionQuality, wasTimed);
            }

            return;
        }

        BeginCurrentDelivery();
    }

    public void ReportCollision(float impactSpeed)
    {
        if (!IsCarryingPackage || impactSpeed <= 0f)
        {
            return;
        }

        float penalty = Mathf.Clamp(
            impactSpeed * penaltyPerImpactSpeed,
            minimumCollisionPenalty,
            maximumCollisionPenalty
        );

        currentFoodQuality = Mathf.Max(
            0f,
            currentFoodQuality - penalty
        );
    }

}