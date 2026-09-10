using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalArrestSequence : MonoBehaviour
{
    [SerializeField] private Transform[] policeCars;
    [SerializeField] private Transform[] stoppingPoints;

    [Header("Timing")]
    [SerializeField] private float entranceDuration = 2f;
    [SerializeField] private float pauseAfterStopping = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource sirenSource;

    [Header("Screen Effects")]
    [SerializeField] private Image policeFlashOverlay;
    [SerializeField] private Image blackFadeOverlay;
    [SerializeField] private CanvasGroup blackFadeGroup;
    [SerializeField] private Image restartFadeOverlay;
    [SerializeField] private float flashInterval = 0.18f;
    [SerializeField] private float flashAlpha = 0.12f;
    [SerializeField] private float blackFadeDuration = 1f;
    [SerializeField] private float restartFadeDuration = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] private float finalBlackAlpha = 0.75f;

    private Coroutine flashCoroutine;
    private bool restarting;

    public bool IsPlaying { get; private set; }

    private void Update()
    {
        bool pressedEnter =
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter);

        bool endingScreenReady =
            blackFadeGroup != null &&
            blackFadeGroup.interactable;

        if (endingScreenReady &&
            pressedEnter &&
            !restarting)
        {
            PlayAgain();
        }
    }

    private void Awake()
    {
        SetImageAlpha(policeFlashOverlay, 0f);
        SetImageAlpha(restartFadeOverlay, 0f);

        if (blackFadeGroup == null && blackFadeOverlay != null)
        {
            blackFadeGroup =
                blackFadeOverlay.GetComponent<CanvasGroup>();

            if (blackFadeGroup == null)
            {
                blackFadeGroup =
                    blackFadeOverlay.gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (blackFadeOverlay != null)
        {
            SetImageAlpha(blackFadeOverlay, finalBlackAlpha);
        }

        if (blackFadeGroup != null)
        {
            blackFadeGroup.alpha = 0f;
            blackFadeGroup.interactable = false;
            blackFadeGroup.blocksRaycasts = false;
        }
    }

    public void Play(Action onComplete)
    {
        if (IsPlaying)
        {
            return;
        }

        if (policeCars.Length != stoppingPoints.Length)
        {
            Debug.LogError(
                "Every police car needs a stopping point."
            );

            onComplete?.Invoke();
            return;
        }

        StartCoroutine(PlaySequence(onComplete));
    }

    private IEnumerator PlaySequence(Action onComplete)
    {
        IsPlaying = true;

        Vector3[] startingPositions =
            new Vector3[policeCars.Length];

        for (int i = 0; i < policeCars.Length; i++)
        {
            startingPositions[i] =
                policeCars[i].position;

            policeCars[i].gameObject.SetActive(true);
        }

        if (sirenSource != null)
        {
            sirenSource.Play();
        }

        if (policeFlashOverlay != null)
        {
            flashCoroutine = StartCoroutine(FlashPoliceLights());
        }

        float elapsed = 0f;

        while (elapsed < entranceDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsed / entranceDuration
            );

            // Makes the cars slow down smoothly
            float smoothProgress =
                Mathf.SmoothStep(0f, 1f, progress);

            for (int i = 0; i < policeCars.Length; i++)
            {
                policeCars[i].position = Vector3.Lerp(
                    startingPositions[i],
                    stoppingPoints[i].position,
                    smoothProgress
                );
            }

            yield return null;
        }

        for (int i = 0; i < policeCars.Length; i++)
        {
            policeCars[i].position =
                stoppingPoints[i].position;
        }

        yield return new WaitForSeconds(
            pauseAfterStopping
        );

        if (sirenSource != null)
        {
            sirenSource.Stop();
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        SetImageAlpha(policeFlashOverlay, 0f);

        IsPlaying = false;
        onComplete?.Invoke();
    }

    private IEnumerator FlashPoliceLights()
    {
        Color red = new Color(1f, 0f, 0f, flashAlpha);
        Color blue = new Color(0f, 0.25f, 1f, flashAlpha);
        bool showRed = true;

        while (true)
        {
            policeFlashOverlay.color = showRed ? red : blue;
            showRed = !showRed;
            yield return new WaitForSeconds(flashInterval);
        }
    }

    public void FadeToBlack(Action onComplete)
    {
        if (blackFadeGroup == null)
        {
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(FadeToBlackSequence(onComplete));
    }

    private IEnumerator FadeToBlackSequence(Action onComplete)
    {
        IsPlaying = true;
        blackFadeGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < blackFadeDuration)
        {
            elapsed += Time.deltaTime;

            blackFadeGroup.alpha = Mathf.Lerp(
                0f,
                1f,
                Mathf.Clamp01(elapsed / blackFadeDuration)
            );

            yield return null;
        }

        blackFadeGroup.alpha = 1f;
        blackFadeGroup.interactable = true;
        blackFadeGroup.blocksRaycasts = true;
        IsPlaying = false;
        onComplete?.Invoke();
    }

    public void PlayAgain()
    {
        if (restarting)
        {
            return;
        }

        restarting = true;
        StartCoroutine(RestartGameSequence());
    }

    private IEnumerator RestartGameSequence()
    {
        if (restartFadeOverlay == null)
        {
            ReloadScene();
            yield break;
        }

        restartFadeOverlay.raycastTarget = true;

        Color colour = restartFadeOverlay.color;
        colour.a = 0f;
        restartFadeOverlay.color = colour;

        float elapsed = 0f;

        while (elapsed < restartFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            colour.a = Mathf.Lerp(
                0f,
                1f,
                Mathf.Clamp01(elapsed / restartFadeDuration)
            );

            restartFadeOverlay.color = colour;
            yield return null;
        }

        colour.a = 1f;
        restartFadeOverlay.color = colour;

        ReloadScene();
    }

    private void ReloadScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color colour = image.color;
        colour.a = alpha;
        image.color = colour;
        image.raycastTarget = false;
    }
}
