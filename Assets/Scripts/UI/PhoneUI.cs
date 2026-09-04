using System;
using TMPro;
using UnityEngine;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private UnityEngine.UI.Button buyButton;
    [SerializeField] private UnityEngine.UI.Button skipButton;
    [SerializeField] private TMP_Text buyButtonText;
    [SerializeField] private TMP_Text skipButtonText;

    private string[] currentLines;
    private int currentLineIndex;
    private bool isShowingDialogueSequence;
    private Action onSequenceComplete;

    /* Runs every frame, watches for Enter key*/
    private void Update()
    {
        if(isShowingDialogueSequence && Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
    }

    /* Shows the first line of dialogue*/
    public void ShowDialogueSequence(string[] lines, Action onComplete = null, string speaker = "BOSS")
    {
        if(lines == null || lines.Length < 1)
        {
            onComplete?.Invoke();
            return;
        }

        if(titleText != null)
        {
            titleText.text = speaker;
        }

        currentLines = lines;
        currentLineIndex = 0;
        isShowingDialogueSequence = true;
        onSequenceComplete = onComplete;

        orderText.text = currentLines[currentLineIndex];
    }

    /*Advances dialogue to next line (if end - turn off sequence and run function onComplete*/
    private void AdvanceDialogue()
    {
        currentLineIndex++;

        if(currentLineIndex >= currentLines.Length)
        {
            isShowingDialogueSequence = false;
            onSequenceComplete?.Invoke();
            return;
        }
        orderText.text = currentLines[currentLineIndex];
    }

    public void ShowPickup(
        int deliveryNumber,
        int totalDeliveries,
        DeliveryRoute route)
    {
        if(titleText != null)
        {
            titleText.text = "ORDER";
        }

        orderText.text =
            "ORDER " + deliveryNumber + "/" + totalDeliveries +
            "\n\nPICKUP: " + route.PickupName +
            "\nDELIVER TO: " + route.DestinationName +
            "\nORDER: " + route.OrderName + 
            "\nDRIVER: Delivery Guy #12";

        if (!string.IsNullOrEmpty(route.BossLine))
        {
            orderText.text += "\n\n" + route.BossLine;
        }
    }

    public void ShowDropOff(
        int deliveryNumber,
        int totalDeliveries,
        DeliveryRoute route,
        float foodQuality)
    {
        if(titleText != null)
        {
            titleText.text = "ORDER";
        }

        orderText.text =
            "ORDER " +
            deliveryNumber + "/" + totalDeliveries +
            "\n\nGot it? Great. " + route.DestinationName + " is waiting for " +
            route.OrderName +
            ".\n\nFood quality: " +
            foodQuality.ToString("F0") + "%";

        if (route.IsTimedDelivery)
        {
            orderText.text +=
                "\nDeliver within " +
                route.TargetDeliveryTime.ToString("F0") +
                " seconds!";
        }
    }

    public void ShowCompleted(
        float deliveryTime,
        float foodQuality,
        bool wasTimed,
        int totalScore)
    {
        if(titleText != null)
        {
            titleText.text = "ORDER";
        }

        orderText.text =
            "That's the last one. Good work today." +
            "\n\nFinal score: " + totalScore;

        if (wasTimed)
        {
            orderText.text +=
                "\n\nFinal delivery: " +
                deliveryTime.ToString("F1") +
                "s | Quality: " +
                foodQuality.ToString("F0") + "%";
        }
        else
        {
            orderText.text +=
                "\n\nFinal quality: " +
                foodQuality.ToString("F0") + "%";
        }
    }

    /* Shows buy and skip buttons for the energy drink prompt*/
    public void ShowEnergyDrinkPrompt(float cost, Action onBuy, Action onSkip)
    {
        if(titleText != null) titleText.text = "BOSS";

        orderText.text = "Buy an energy drink before next delivery?" +
        "\n\nCost: $" + cost.ToString("F0");

        if(buyButton != null)
        {
            buyButton.gameObject.SetActive(true);
            buyButtonText.text = "BUY";
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                HideButtons();
                onBuy?.Invoke();
            });
        }

        if(skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
            if(skipButtonText != null) skipButtonText.text = "SKIP";
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(() =>
            {
                HideButtons();
                onSkip?.Invoke();
            });
        }
    }

    /* Forces buy by making both buttons confirm the purchase*/
    public void ShowEnergyDrinkForced(float cost, Action onBuy)
    {
        if(titleText != null) titleText.text = "BOSS";

        orderText.text = "Buy an energy drink before next delivery?" +
        "\n\nCost: $" + cost.ToString("F0");

        if(buyButton != null)
        {
            buyButton.gameObject.SetActive(true);
            if(buyButtonText != null) buyButtonText.text = "BUY";
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                HideButtons();
                onBuy?.Invoke();
            });
        }

        if(skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
            if(skipButtonText != null) skipButtonText.text = "BUY";
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(() =>
            {
                HideButtons();
                onBuy?.Invoke();
            });
        }
    }

    /* Hides both buttons after a choice is made*/
    private void HideButtons()
    {
        buyButton?.gameObject.SetActive(false);
        skipButton?.gameObject.SetActive(false);
    }
}