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
        DeliveryRoute route)
    {
        orderText.text =
            "ORDER " +
            deliveryNumber + "/" + totalDeliveries +
            "\n\nGot it? Great. The customer is waiting at " +
            route.DestinationName +
            ".";
    }

    public void ShowCompleted()
    {
        orderText.text =
            "That's the last one. " +
            "Good work today.";
    }
}