using UnityEngine;

public class DeliveryRoute : MonoBehaviour
{
    [Header("Route Points")]
    [SerializeField] private DeliveryPoint pickupPoint;
    [SerializeField] private DeliveryPoint dropOffPoint;

    [Header("Timing")]
    [SerializeField] private bool timedDelivery = true;
    [SerializeField] private float targetDeliveryTime = 60f;

    [Header("Rewards")]
    [SerializeField] private float baseTip = 20f;
    [SerializeField] private int baseScore = 1000;

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