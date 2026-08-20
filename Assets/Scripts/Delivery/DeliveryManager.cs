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

        Debug.Log(
            "Delivery " + CurrentDeliveryNumber +
            " of " + TotalDeliveries +
            ": collect the package."
        );

        LogProgress();
    }

    private void CollectPackage()
    {
        currentState = DeliveryState.CarryingPackage;
        deliveryElapsedTime = 0f;
        timerRunning = CurrentDelivery.IsTimedDelivery;

        CurrentDelivery.ShowDropOff();

        Debug.Log(
            "Delivery " + CurrentDeliveryNumber +
            ": package collected. Drive to the delivery point."
        );

        Debug.Log(
            "Starting food quality: " +
            currentFoodQuality.ToString("F1") +
            "%"
        );

        if (timerRunning)
        {
            Debug.Log(
                "Timer started. Target time: " +
                CurrentDelivery.TargetDeliveryTime.ToString("F1") +
                " seconds."
            );
        }
        else
        {
            Debug.Log("This delivery is untimed.");
        }
    }

    private void CompleteCurrentDelivery()
    {
        timerRunning = false;

        if (CurrentDelivery.IsTimedDelivery)
        {
            Debug.Log(
                "Delivery time: " +
                deliveryElapsedTime.ToString("F1") +
                " seconds."
            );
        }

        Debug.Log(
            "Final food quality: " +
            currentFoodQuality.ToString("F1") +
            "%"
        );

        CurrentDelivery.Hide();
        currentDeliveryIndex++;

        Debug.Log(
            "Delivery completed. Progress: " +
            CompletedDeliveries + "/" + TotalDeliveries
        );

        if (currentDeliveryIndex >= deliveries.Length)
        {
            currentState = DeliveryState.Completed;
            Debug.Log("All deliveries completed!");
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

        Debug.Log(
            "Collision detected. Impact: " +
            impactSpeed.ToString("F1") +
            " | Food quality penalty: " +
            penalty.ToString("F1") +
            " | Food quality: " +
            currentFoodQuality.ToString("F1") +
            "%"
        );
    }

    private void LogProgress()
    {
        Debug.Log(
            "Current delivery: " + CurrentDeliveryNumber +
            " | Completed: " + CompletedDeliveries +
            " | Total: " + TotalDeliveries
        );
    }
}