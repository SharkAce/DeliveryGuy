using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NightOverlayController : MonoBehaviour
{
    [SerializeField] private Image overlayImage;

    [Header("Colours")]
    [SerializeField]
    private Color dayColour =
        new Color(0f, 0f, 0f, 0f);

    [SerializeField]
    private Color nightColour =
        new Color(0.04f, 0.08f, 0.24f, 0.55f);

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 2f;

    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (overlayImage == null)
        {
            overlayImage = GetComponent<Image>();
        }

        if (overlayImage != null)
        {
            overlayImage.color = dayColour;
            overlayImage.raycastTarget = false;
        }
    }

    public void UpdateLighting(
        int completedDeliveries,
        int totalDeliveries)
    {
        if (overlayImage == null)
        {
            return;
        }

        float progress = totalDeliveries <= 0
            ? 0f
            : (float)completedDeliveries / totalDeliveries;

        Color targetColour =
            Color.Lerp(dayColour, nightColour, progress);

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(
            ChangeLighting(targetColour)
        );
    }

    private IEnumerator ChangeLighting(Color targetColour)
    {
        Color startingColour = overlayImage.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / transitionDuration
            );

            overlayImage.color = Color.Lerp(
                startingColour,
                targetColour,
                progress
            );

            yield return null;
        }

        overlayImage.color = targetColour;
        transitionCoroutine = null;
    }
}