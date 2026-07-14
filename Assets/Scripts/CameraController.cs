using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public GameObject car;

    void Update()
    {
        Vector3 newPosition = car.transform.position;
        newPosition.z = -10f;
        transform.position = newPosition;
    }
}