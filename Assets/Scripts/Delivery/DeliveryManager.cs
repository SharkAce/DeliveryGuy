using UnityEngine;
using System.Collections;

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

    [SerializeField] private HUDDisplay hudDisplay;

    [Header("Day and Night")]
    [SerializeField]
    private NightOverlayController nightOverlay;

    [Header("Final Arrest")]
    [SerializeField]
    private FinalArrestSequence finalArrestSequence;

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

    [Header("Music")]
    [SerializeField] private SoundtrackManager soundtrackManager;

    private int currentDeliveryIndex;
    private DeliveryState currentState;

    private float deliveryElapsedTime;
    private bool timerRunning;
    private float currentFoodQuality;
    private float currentDrivingScore;

    private float lastDeliveryTip;
    private int lastDeliveryScore;
    private int totalScore;
    private float totalTips;
    private float timerScale = 1f;

    private bool IsCarryingPackage
    {
        get
        {
            return currentState == DeliveryState.CarryingPackage;
        }
    }

    public bool IsDialogueActive
    {
        get
        {
            bool phoneDialogueActive =
                phoneUI != null && phoneUI.IsShowingDialogue;

            bool arrestSequenceActive =
                finalArrestSequence != null &&
                finalArrestSequence.IsPlaying;

            return phoneDialogueActive || arrestSequenceActive;
        }
    }

    private DeliveryRoute CurrentDelivery
    {
        get { return deliveries[currentDeliveryIndex]; }
    }

    private void Start()
    {
        /* Auto-assign major UI/Manager references*/
        if (hudDisplay == null) hudDisplay = FindObjectOfType<HUDDisplay>();
        if (phoneUI == null) phoneUI = FindObjectOfType<PhoneUI>();
        if (objectiveArrow == null) objectiveArrow = FindObjectOfType<ObjectiveArrow>();
        if (minimapMarkers == null) minimapMarkers = FindObjectOfType<MinimapDeliveryMarkers>();
        if (nightOverlay == null) nightOverlay = FindObjectOfType<NightOverlayController>();
        if (finalArrestSequence == null) finalArrestSequence = FindObjectOfType<FinalArrestSequence>();

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
        totalTips = 0;

        if (nightOverlay != null)
        {
            nightOverlay.UpdateLighting(
                0,
                deliveries.Length
            );
        }

        if (hudDisplay != null)
        {
            hudDisplay.UpdateMoney(0f);
            hudDisplay.UpdateScore(0);
            hudDisplay.ShowTimer();
        }

        if (soundtrackManager == null)
        {
            soundtrackManager = FindObjectOfType<SoundtrackManager>();
        }

        BeginCurrentDelivery();
    }

    private void Update()
    {
        if (timerRunning)
        {
            deliveryElapsedTime += Time.deltaTime * timerScale;
            if (hudDisplay != null)
            {
                float remaining = Mathf.Max(0f,
                    CurrentDelivery.TargetDeliveryTime - deliveryElapsedTime);

                if (CurrentDelivery.IsTimedDelivery)
                {
                    hudDisplay.UpdateTimer(remaining);
                }

                if (IsCarryingPackage)
                {
                    hudDisplay.UpdateFoodQuality(currentFoodQuality);
                }
            }
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

        if (hudDisplay != null)
        {
            hudDisplay.ShowEmptyFoodQuality();
        }

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

        if (soundtrackManager != null)
        {
            soundtrackManager.PlayForDelivery(currentDeliveryIndex + 1);
        }

        if (objectiveArrow != null)
        {
            objectiveArrow.ClearTarget();
        }

        UpdateTimerDisplay();

        if (phoneUI != null &&
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

    private void UpdateTimerDisplay()
    {
        if (hudDisplay == null)
        {
            return;
        }

        hudDisplay.ShowTimer();

        if (CurrentDelivery.IsTimedDelivery)
        {
            hudDisplay.UpdateTimer(CurrentDelivery.TargetDeliveryTime);
        }
        else
        {
            hudDisplay.ShowUntimedTimer();
        }
    }

    /* Runs after pre-delivery dialogue, with the drink action in delivery 6*/
    private void OnDialogueComplete()
    {
        if (CurrentDelivery.HasEnergyDrinkPrompt && phoneUI != null)
        {
            float cost = totalTips * 0.51f;

            phoneUI.ShowEnergyDrinkPrompt(
                cost,
                onBuy: () => BuyEnergyDrink(cost),
                onSkip: HandleEnergyDrinkSkip
            );

            return;
        }

        ProceedToPickup();
    }

    private void BuyEnergyDrink(float cost)
    {
        SpendTips(cost);
        ProceedToPickup();
    }

    private void HandleEnergyDrinkSkip()
    {
        if (CurrentDelivery.SkipDialogueLines != null &&
            CurrentDelivery.SkipDialogueLines.Length > 0)
        {
            phoneUI.ShowDialogueSequence(
                CurrentDelivery.SkipDialogueLines,
                ShowForcedEnergyDrink
            );
        }
        else
        {
            ShowForcedEnergyDrink();
        }
    }

    private void ShowForcedEnergyDrink()
    {
        float forcedCost = totalTips * 0.51f;

        phoneUI.ShowEnergyDrinkForced(
            forcedCost,
            onBuy: () => BuyEnergyDrink(forcedCost)
        );
    }

    /* Shows pickup UI and sets arrow target*/
    private void ProceedToPickup()
    {
        CurrentDelivery.ShowPickup();

        deliveryElapsedTime = 0f;
        timerScale = 1f;
        timerRunning = true;

        UpdateTimerDisplay();

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
        CurrentDelivery.ShowDropOff();

        if (hudDisplay != null)
        {
            hudDisplay.UpdateFoodQuality(currentFoodQuality);
        }

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

        if (hudDisplay != null)
        {
            hudDisplay.ShowEmptyFoodQuality();
        }

        CurrentDelivery.Hide();

        bool isFinalDelivery =
            currentDeliveryIndex == deliveries.Length - 1;

        if (isFinalDelivery && finalArrestSequence != null)
        {
            if (soundtrackManager != null)
            {
                soundtrackManager.PlayPoliceTrack();
            }

            finalArrestSequence.Play(ShowArrivalDialogue);
        }
        else
        {
            ShowArrivalDialogue();
        }
    }

    private void ShowArrivalDialogue()
    {
        if (phoneUI != null &&
            CurrentDelivery.ArrivalLines != null &&
            CurrentDelivery.ArrivalLines.Length > 0)
        {
            phoneUI.ShowDialogueSequence(
                CurrentDelivery.ArrivalLines,
                OnArrivalDialogueComplete,
                CurrentDelivery.ArrivalSpeakerName
            );
        }
        else
        {
            OnArrivalDialogueComplete();
        }
    }

    private void OnArrivalDialogueComplete()
    {
        CalculateDeliveryRewards();
        if (hudDisplay != null)
        {
            hudDisplay.UpdateMoney(totalTips);
            hudDisplay.UpdateScore(totalScore);
            hudDisplay.ShowPositivePopup(lastDeliveryTip);
        }
        currentDeliveryIndex++;

        if (nightOverlay != null)
        {
            nightOverlay.UpdateLighting(
                currentDeliveryIndex,
                deliveries.Length
            );
        }

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
                if (finalArrestSequence != null)
                {
                    finalArrestSequence.FadeToBlack(() =>
                    {
                        phoneUI.ShowCompleted(totalScore);
                    });
                }
                else
                {
                    phoneUI.ShowCompleted(totalScore);
                }
            }

            return;
        }

        BeginCurrentDelivery();
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

        totalTips += lastDeliveryTip;

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
        if (!IsCarryingPackage ||
            IsDialogueActive ||
            impactSpeed <= 0f)
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

        if (hudDisplay != null)
        {
            hudDisplay.ShowNegativePopup(foodPenalty);
        }

        currentDrivingScore = Mathf.Max(
            0f,
            currentDrivingScore - drivingPenalty
        );
    }

    /* Deducts purchase cost from tip total and updates HUD */
    private void SpendTips(float amount)
    {
        totalTips = Mathf.Max(0f, totalTips - amount);

        if (hudDisplay != null)
        {
            hudDisplay.UpdateMoney(totalTips);
            hudDisplay.ShowMoneySpentPopup(amount);
        }

        if (CurrentDelivery.HasEnergyDrinkPrompt)
        {
            StartCoroutine(SlowTimer());
        }
    }

    /*Wait 5 seconds, then slow down timer for energy drink effect*/
    private IEnumerator SlowTimer()
    {
        yield return new WaitForSeconds(5f);
        if (hudDisplay != null)
        {
            hudDisplay.ShowEnergyDrinkBanner();
        }
        timerScale = 0.05f;
    }
}
