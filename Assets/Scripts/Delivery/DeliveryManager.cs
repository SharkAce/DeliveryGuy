using System.Collections.Generic;
using UnityEngine;

public enum DrivingRating
{
    Horrible,
    Bad,
    Average,
    Good,
    Excellent
}

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

    [Header("Driving Score")]
    [SerializeField] private float startingDrivingScore = 100f;
    [SerializeField] private float drivingPenaltyMultiplier = 1f;

    [Header("Score Weights")]
    [Range(0f, 1f)]
    [SerializeField] private float timeWeight = 0.4f;

    [Range(0f, 1f)]
    [SerializeField] private float foodQualityWeight = 0.3f;

    [Range(0f, 1f)]
    [SerializeField] private float drivingWeight = 0.3f;

    private int currentDeliveryIndex;
    private DeliveryState currentState;

    private float deliveryElapsedTime;
    private bool timerRunning;
    private float currentFoodQuality;
    private float currentDrivingScore;

    private float lastDeliveryTip;
    private int lastDeliveryScore;
    private int totalScore;

    private readonly List<DeliveryResult> deliveryResults =
        new List<DeliveryResult>();

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

    public float CurrentDrivingScore
    {
        get { return currentDrivingScore; }
    }

    public float LastDeliveryTip
    {
        get { return lastDeliveryTip; }
    }

    public int LastDeliveryScore
    {
        get { return lastDeliveryScore; }
    }

    public int TotalScore
    {
        get { return totalScore; }
    }

    public DeliveryResult[] DeliveryResults
    {
        get { return deliveryResults.ToArray(); }
    }

    public DrivingRating CurrentDrivingRating
    {
        get
        {
            if (currentDrivingScore >= 90f)
            {
                return DrivingRating.Excellent;
            }

            if (currentDrivingScore >= 75f)
            {
                return DrivingRating.Good;
            }

            if (currentDrivingScore >= 50f)
            {
                return DrivingRating.Average;
            }

            if (currentDrivingScore >= 25f)
            {
                return DrivingRating.Bad;
            }

            return DrivingRating.Horrible;
        }
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
        totalScore = 0;
        deliveryResults.Clear();

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

        currentDrivingScore = Mathf.Clamp(
            startingDrivingScore,
            0f,
            100f
        );

        if(objectiveArrow != null)
        {
            objectiveArrow.ClearTarget();
        }

        if(phoneUI != null &&
            CurrentDelivery.DialogueLines != null &&
            CurrentDelivery.DialogueLines.Length > 0)
        {
            phoneUI.ShowDialogueSequence(
                CurrentDelivery.DialogueLines,
                OnDialogueComplete
            );
        }
        else
        {
            OnDialogueComplete();
        }
    }

    private void OnDialogueComplete()
    {
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

        if(phoneUI != null &&
            CurrentDelivery.ArrivalLines != null &&
            CurrentDelivery.ArrivalLines.Length > 0)
        {
            phoneUI.ShowDialogueSequence(
                CurrentDelivery.ArrivalLines,
                OnArrivalDialogueComplete
            );
        }
        else
        {
            OnArrivalDialogueComplete();
        }

    }

    private void OnArrivalDialogueComplete()
    {
        float completionTime = deliveryElapsedTime;
        float completionQuality = currentFoodQuality;
        bool wasTimed = CurrentDelivery.IsTimedDelivery;

        CalculateDeliveryRewards();
        StoreDeliveryResult();

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
                phoneUI.ShowCompleted(
                    completionTime,
                    completionQuality,
                    wasTimed,
                    totalScore
                );
            }

            return;
        }

        BeginCurrentDelivery();
    }

    private void StoreDeliveryResult()
    {
        float targetTime = CurrentDelivery.IsTimedDelivery
            ? CurrentDelivery.TargetDeliveryTime
            : 0f;

        DeliveryResult result = new DeliveryResult(
            CurrentDeliveryNumber,
            deliveryElapsedTime,
            targetTime,
            currentFoodQuality,
            currentDrivingScore,
            CurrentDrivingRating,
            lastDeliveryTip,
            lastDeliveryScore,
            totalScore
        );

        deliveryResults.Add(result);
    }

    private void CalculateDeliveryRewards()
    {
        float timeMultiplier = 1f;

        if (CurrentDelivery.IsTimedDelivery)
        {
            float safeElapsedTime = Mathf.Max(
                0.1f,
                deliveryElapsedTime
            );

            timeMultiplier = Mathf.Clamp(
                CurrentDelivery.TargetDeliveryTime /
                safeElapsedTime,
                0.25f,
                1.25f
            );
        }

        float foodMultiplier =
            Mathf.Clamp01(currentFoodQuality / 100f);

        float drivingMultiplier =
            Mathf.Clamp01(currentDrivingScore / 100f);

        lastDeliveryTip =
            CurrentDelivery.BaseTip *
            timeMultiplier *
            foodMultiplier *
            drivingMultiplier;

        lastDeliveryTip = Mathf.Max(
            0f,
            lastDeliveryTip
        );

        float totalWeight =
            timeWeight +
            foodQualityWeight +
            drivingWeight;

        if (totalWeight <= 0f)
        {
            totalWeight = 1f;
        }

        float performance =
            (
                timeMultiplier * timeWeight +
                foodMultiplier * foodQualityWeight +
                drivingMultiplier * drivingWeight
            ) / totalWeight;

        lastDeliveryScore = Mathf.Max(
            0,
            Mathf.RoundToInt(
                CurrentDelivery.BaseScore *
                performance
            )
        );

        totalScore += lastDeliveryScore;
    }

    public void ReportCollision(float impactSpeed)
    {
        if (!IsCarryingPackage || impactSpeed <= 0f)
        {
            return;
        }

        float foodPenalty = Mathf.Clamp(
            impactSpeed * penaltyPerImpactSpeed,
            minimumCollisionPenalty,
            maximumCollisionPenalty
        );

        float drivingPenalty =
            foodPenalty * drivingPenaltyMultiplier;

        currentFoodQuality = Mathf.Max(
            0f,
            currentFoodQuality - foodPenalty
        );

        currentDrivingScore = Mathf.Max(
            0f,
            currentDrivingScore - drivingPenalty
        );
    }

}