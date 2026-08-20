using TMPro;
using UnityEngine;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text orderText;

    public void ShowPickup(
        int deliveryNumber,
        int totalDeliveries,
        DeliveryRoute route)
    {
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

    public void ShowCompleted(float deliveryTime, float foodQuality, bool wasTimed)
    {
        orderText.text = "That's the last one. Good work today.";

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