using UnityEngine;

public class MinimapMarkerFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            0f
        );
    }
}