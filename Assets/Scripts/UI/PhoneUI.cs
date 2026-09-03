using System;
using TMPro;
using UnityEngine;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text titleText;

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
            "ORDER " +
            deliveryNumber + "/" + totalDeliveries +
            "\n\nNew order just came in from " +
            route.PickupName +
            ". It's going to " +
            route.DestinationName +
            ".";

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
            "\n\nGot it? Great. The customer is waiting at " +
            route.DestinationName +
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
}