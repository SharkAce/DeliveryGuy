using UnityEngine;

public class DeliveryCollisionReporter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeliveryManager deliveryManager;

    [Header("Collision Detection")]
    [SerializeField] private float minimumImpactSpeed = 1.5f;
    [SerializeField] private float reportCooldown = 0.5f;

    private float nextAllowedReportTime;

    private void Awake()
    {
        if (deliveryManager == null)
        {
            deliveryManager = FindObjectOfType<DeliveryManager>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (deliveryManager == null)
        {
            return;
        }

        if (Time.time < nextAllowedReportTime)
        {
            return;
        }

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
        {
            return;
        }

        deliveryManager.ReportCollision(impactSpeed);
        nextAllowedReportTime = Time.time + reportCooldown;
    }
}