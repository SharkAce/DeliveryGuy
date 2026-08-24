using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectiveArrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform car;

    [Header("Position")]
    [SerializeField] private float distanceFromCar = 1f;
    [SerializeField] private float hideDistance = 1.5f;

    [Header("Rotation")]
    [SerializeField] private float spriteAngleOffset = 0f;

    private Transform target;
    private SpriteRenderer arrowRenderer;

    private void Awake()
    {
        arrowRenderer = GetComponent<SpriteRenderer>();
        arrowRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (car == null || target == null)
        {
            arrowRenderer.enabled = false;
            return;
        }

        Vector2 direction = target.position - car.position;
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget <= hideDistance)
        {
            arrowRenderer.enabled = false;
            return;
        }

        arrowRenderer.enabled = true;

        Vector2 normalizedDirection = direction.normalized;

        transform.position = new Vector3(
            car.position.x + normalizedDirection.x * distanceFromCar,
            car.position.y + normalizedDirection.y * distanceFromCar,
            0f
        );

        float angle = Mathf.Atan2(
            normalizedDirection.y,
            normalizedDirection.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angle + spriteAngleOffset
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
        arrowRenderer.enabled = false;
    }
}