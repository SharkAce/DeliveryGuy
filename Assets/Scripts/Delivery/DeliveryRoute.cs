using UnityEngine;

public class DeliveryRoute : MonoBehaviour
{
    [Header("Order Information")]
    [SerializeField] private string pickupName = "Restaurant";
    [SerializeField] private string destinationName = "Customer";

    [Header("Route Points")]
    [SerializeField] private DeliveryPoint pickupPoint;
    [SerializeField] private DeliveryPoint dropOffPoint;

    [Header("Boss Dialogue")]
    [SerializeField] [TextArea] private string bossLine = "";

    public string PickupName
    {
        get { return pickupName; }
    }

    public string DestinationName
    {
        get { return destinationName; }
    }

    public string BossLine
    {
        get { return bossLine; }
    }

    public DeliveryPoint PickupPoint
    {
        get { return pickupPoint; }
    }

    public DeliveryPoint DropOffPoint
    {
        get { return dropOffPoint; }
    }

    public void Initialize(DeliveryManager manager)
    {
        pickupPoint.Initialize(manager);
        dropOffPoint.Initialize(manager);
    }

    public void ShowPickup()
    {
        gameObject.SetActive(true);
        pickupPoint.gameObject.SetActive(true);
        dropOffPoint.gameObject.SetActive(false);
    }

    public void ShowDropOff()
    {
        pickupPoint.gameObject.SetActive(false);
        dropOffPoint.gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}