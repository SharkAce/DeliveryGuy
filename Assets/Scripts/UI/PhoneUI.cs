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
    [SerializeField] private AudioClip advanceSound;
    [SerializeField] private AudioSource phoneSounds;

    private string[] currentLines;
    private int currentLineIndex;
    private bool isShowingDialogueSequence;
    private Action onSequenceComplete;

    /* Exposes dialogue state for car input blocking */
    public bool IsShowingDialogue
    {
        get { return isShowingDialogueSequence; }
    }

    /* Runs every frame, watches for Enter key*/
    private void Update()
    {
        if (isShowingDialogueSequence && Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }

        if (buyButton != null &&
        buyButton.gameObject.activeInHierarchy &&
        (Input.GetKeyDown(KeyCode.Alpha1) ||
         Input.GetKeyDown(KeyCode.Keypad1)))
        {
            buyButton.onClick.Invoke();
        }

        if (skipButton != null &&
            skipButton.gameObject.activeInHierarchy &&
            (Input.GetKeyDown(KeyCode.Alpha2) ||
             Input.GetKeyDown(KeyCode.Keypad2)))
        {
            skipButton.onClick.Invoke();
        }
    }

    public void OnBubbleClick()
    {
        if (isShowingDialogueSequence)
        {
            AdvanceDialogue();
        }
    }

    /* Shows the first line of dialogue*/
    public void ShowDialogueSequence(string[] lines, Action onComplete = null, string speaker = "BOSS")
    {
        phoneSounds.PlayOneShot(advanceSound);
        if (lines == null || lines.Length < 1)
        {
            onComplete?.Invoke();
            return;
        }

        if (titleText != null)
        {
            titleText.text = speaker;
        }

        currentLines = lines;
        currentLineIndex = 0;
        isShowingDialogueSequence = true;
        onSequenceComplete = onComplete;

        orderText.text = currentLines[currentLineIndex] + "\n\n[Press ENTER to continue]";
    }

    /*Advances dialogue to next line (if end - turn off sequence and run function onComplete*/
    private void AdvanceDialogue()
    {
        phoneSounds.PlayOneShot(advanceSound);
        currentLineIndex++;
        if (currentLineIndex >= currentLines.Length)
        {
            isShowingDialogueSequence = false;
            onSequenceComplete?.Invoke();
            return;
        }
        orderText.text = currentLines[currentLineIndex] + "\n\n[Press ENTER to continue]";
    }

    public void ShowPickup(
        int deliveryNumber,
        int totalDeliveries,
        DeliveryRoute route)
    {
        phoneSounds.PlayOneShot(advanceSound);
        if (titleText != null)
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
        phoneSounds.PlayOneShot(advanceSound);
        if (titleText != null)
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

    public void ShowCompleted(int totalScore)
    {
        phoneSounds.PlayOneShot(advanceSound);
        if (titleText != null)
        {
            titleText.text = "EVIDENCE";
        }

        orderText.text =
            "Delivery complete." +
            "\nUnfortunately, so is our investigation." +
            "\n\nYour delivery contained: HUMAN ORGANS" +
            "\nEmployer status: UNKNOWN" +
            "\n\nFinal score: " + totalScore;
    }

    /* Shows buy and skip buttons for the energy drink prompt*/
    public void ShowEnergyDrinkPrompt(float cost, Action onBuy, Action onSkip)
    {
        if (titleText != null) titleText.text = "BOSS";

        orderText.text = "Buy an energy drink before next delivery?" +
        "\n\nCost: $" + cost.ToString("F0");

        SetupButton(buyButton, buyButtonText, "[1] BUY", onBuy);
        SetupButton(skipButton, skipButtonText, "[2] SKIP", onSkip);
    }

    /* Forces buy by making both buttons confirm the purchase*/
    public void ShowEnergyDrinkForced(float cost, Action onBuy)
    {
        if (titleText != null) titleText.text = "BOSS";

        orderText.text = "Buy an energy drink before next delivery?" +
        "\n\nCost: $" + cost.ToString("F0");

        SetupButton(buyButton, buyButtonText, "[1] BUY", onBuy);
        SetupButton(skipButton, skipButtonText, "[2] BUY", onBuy);
    }

    private void SetupButton(
        UnityEngine.UI.Button button,
        TMP_Text buttonText,
        string buttonLabel,
        Action action)
    {
        if (button == null) return;

        button.gameObject.SetActive(true);

        if (buttonText != null)
        {
            buttonText.text = buttonLabel;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            HideButtons();
            action?.Invoke();
        });
    }

    /* Hides both buttons after a choice is made*/
    private void HideButtons()
    {
        buyButton?.gameObject.SetActive(false);
        skipButton?.gameObject.SetActive(false);
    }
}