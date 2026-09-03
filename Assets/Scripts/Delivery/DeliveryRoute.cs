using UnityEngine;

public class DeliveryRoute : MonoBehaviour
{
    [Header("Order Information")]
    [SerializeField] private string pickupName = "Restaurant";
    [SerializeField] private string destinationName = "Customer";
    [SerializeField] private string orderName = "Order Name";

    [Header("Route Points")]
    [SerializeField] private DeliveryPoint pickupPoint;
    [SerializeField] private DeliveryPoint dropOffPoint;

    [Header("Boss Dialogue")]
    [SerializeField] [TextArea] private string bossLine = "";

    [Header("Boss Dialogue (advance with Enter)")]
    [SerializeField] [TextArea] private string[] dialogueLines;

    [Header("Arrival Dialogue (customer lines, advance with Enter)")]
    [SerializeField] [TextArea] private string[] arrivalLines;

    [Header("Arrival Speaker Name")]
    [SerializeField] private string arrivalSpeakerName = "CLIENT";

    public string ArrivalSpeakerName
    {
        get { return arrivalSpeakerName; }
    }

    public string[] ArrivalLines
    {
        get { return arrivalLines; }
    }

    public string BossLine
    {
        get { return bossLine; }
    }

    public string[] DialogueLines
    {
        get { return dialogueLines; }
    }

    [Header("Timing")]
    [SerializeField] private bool timedDelivery = true;
    [SerializeField] private float targetDeliveryTime = 60f;

    [Header("Rewards")]
    [SerializeField] private float baseTip = 20f;
    [SerializeField] private int baseScore = 1000;

    public string PickupName
    {
        get { return pickupName; }
    }

    public string DestinationName
    {
        get { return destinationName; }
    }

    public string OrderName
    {
        get { return orderName; }
    }

    public DeliveryPoint PickupPoint
    {
        get { return pickupPoint; }
    }

    public DeliveryPoint DropOffPoint
    {
        get { return dropOffPoint; }
    }

    public bool IsTimedDelivery
    {
        get { return timedDelivery; }
    }

    public float TargetDeliveryTime
    {
        get { return Mathf.Max(1f, targetDeliveryTime); }
    }

    public float BaseTip
    {
        get { return Mathf.Max(0f, baseTip); }
    }

    public int BaseScore
    {
        get { return Mathf.Max(0, baseScore); }
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