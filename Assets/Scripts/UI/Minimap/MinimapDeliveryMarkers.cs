using UnityEngine;

public class MinimapDeliveryMarkers : MonoBehaviour
{
    [System.Serializable]
    private class MarkerPair
    {
        [SerializeField] private GameObject pickupMarker;
        [SerializeField] private GameObject dropOffMarker;

        public GameObject PickupMarker
        {
            get { return pickupMarker; }
        }

        public GameObject DropOffMarker
        {
            get { return dropOffMarker; }
        }
    }

    [SerializeField] private MarkerPair[] deliveryMarkers;

    private void Awake()
    {
        HideAll();
    }

    public void ShowPickup(int deliveryIndex)
    {
        HideAll();

        if (!IsValidIndex(deliveryIndex))
        {
            return;
        }

        GameObject marker =
            deliveryMarkers[deliveryIndex].PickupMarker;

        if (marker != null)
        {
            marker.SetActive(true);
        }
    }

    public void ShowDropOff(int deliveryIndex)
    {
        HideAll();

        if (!IsValidIndex(deliveryIndex))
        {
            return;
        }

        GameObject marker =
            deliveryMarkers[deliveryIndex].DropOffMarker;

        if (marker != null)
        {
            marker.SetActive(true);
        }
    }

    public void HideAll()
    {
        if (deliveryMarkers == null)
        {
            return;
        }

        foreach (MarkerPair pair in deliveryMarkers)
        {
            if (pair.PickupMarker != null)
            {
                pair.PickupMarker.SetActive(false);
            }

            if (pair.DropOffMarker != null)
            {
                pair.DropOffMarker.SetActive(false);
            }
        }
    }

    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= deliveryMarkers.Length)
        {
            Debug.LogError(
                $"No minimap markers configured for delivery {index}."
            );

            return false;
        }

        return true;
    }
}