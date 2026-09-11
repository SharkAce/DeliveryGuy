using System.Collections;
using TMPro;
using UnityEngine;

public class HUDDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text foodQualityText;
    [SerializeField] private GameObject energyDrinkBanner;
    [SerializeField] private TMP_Text dialogueHintText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text moneyPopupText;
    [SerializeField] private TMP_Text qualityPopupText;

    private Coroutine moneyPopupCoroutine;
    private Coroutine qualityPopupCoroutine;
    private Coroutine breatheCoroutine;
    private Coroutine dialogueHintCoroutine;

    private void Start()
    {
        /* Find deactivated UI children by name*/
        if (energyDrinkBanner == null)
        {
            Transform banner = transform.Find("EnergyDrinkBanner");
            if (banner != null) energyDrinkBanner = banner.gameObject;
        }

        if (dialogueHintText == null)
        {
            Transform hint = transform.Find("DialogueHint");
            if (hint != null) dialogueHintText = hint.GetComponent<TMP_Text>();
        }

        if (dialogueHintText != null)
        {
            dialogueHintText.gameObject.SetActive(false);
        }
    }

    /* Update money display */
    public void UpdateMoney(float amount)
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + amount.ToString("F0");
        }
    }

    /* Update food quality display */
    public void UpdateFoodQuality(float quality)
    {
        if (foodQualityText != null)
        {
            foodQualityText.text = "Quality: " + quality.ToString("F0") + "%";
        }
    }

    /* Shows red popup for money spent on energy drinks */
    public void ShowMoneySpentPopup(float amount)
    {
        if (moneyPopupText == null) return;

        if (moneyPopupCoroutine != null)
        {
            StopCoroutine(moneyPopupCoroutine);
        }

        moneyPopupText.text = "-$" + amount.ToString("F0");
        moneyPopupText.color = new Color(0.9f, 0.1f, 0.1f, 1f);
        moneyPopupText.gameObject.SetActive(true);
        moneyPopupCoroutine = StartCoroutine(
            FadePopup(moneyPopupText, true)
        );
    }

    /* Update countdown timer, turns red and breathes when at zero */
    public void UpdateTimer(float remaining)
    {
        if (timerText == null) return;

        remaining = Mathf.Max(0f, remaining);

        int seconds = Mathf.FloorToInt(remaining);
        int milliseconds = Mathf.FloorToInt((remaining - seconds) * 1000f);
        timerText.text = "Time: " + seconds.ToString("00") + ":" + milliseconds.ToString("000");

        if (remaining <= 5f)
        {
            timerText.color = new Color(0.8f, 0f, 0f);
            if (breatheCoroutine == null)
            {
                breatheCoroutine = StartCoroutine(BreatheTimer());
            }
        }
        else
        {
            timerText.color = Color.white;
            if (breatheCoroutine != null)
            {
                StopCoroutine(breatheCoroutine);
                breatheCoroutine = null;
                timerText.fontSize = 28f;
            }
        }
    }

    /* Pulses timer between deep red and light red when overtime */
    private IEnumerator BreatheTimer()
    {
        float speed = 3.5f;
        float elapsed = 0f;
        Color deepRed = new Color(0.6f, 0f, 0f);
        Color lightRed = new Color(1f, 0.4f, 0.4f);

        while (true)
        {
            elapsed += Time.deltaTime * speed;
            float t = (Mathf.Sin(elapsed) + 1f) / 2f;
            timerText.color = Color.Lerp(deepRed, lightRed, t);
            yield return null;
        }
    }

    /* Shows timer when delivery begins */
    public void ShowTimer()
    {
        if (timerText != null)
        {
            timerText.color = Color.white;
            timerText.gameObject.SetActive(true);
        }
    }

    /* Shows green popup for tips earned */
    public void ShowPositivePopup(float amount)
    {
        if (moneyPopupText == null) return;

        if (moneyPopupCoroutine != null)
        {
            StopCoroutine(moneyPopupCoroutine);
        }

        moneyPopupText.text = "+$" + amount.ToString("F0");
        moneyPopupText.color = new Color(0.2f, 0.8f, 0.2f, 1f);
        moneyPopupText.gameObject.SetActive(true);
        moneyPopupCoroutine = StartCoroutine(
            FadePopup(moneyPopupText, true)
        );
    }

    /* Shows red popup for quality penalty from crashes */
    public void ShowNegativePopup(float amount)
    {
        if (qualityPopupText == null) return;

        if (qualityPopupCoroutine != null)
        {
            StopCoroutine(qualityPopupCoroutine);
        }

        qualityPopupText.text =
            "-" + amount.ToString("F0") + "% quality";
        qualityPopupText.color = new Color(0.9f, 0.1f, 0.1f, 1f);
        qualityPopupText.gameObject.SetActive(true);
        qualityPopupCoroutine = StartCoroutine(
            FadePopup(qualityPopupText, false)
        );
    }

    /* Shows energy drink banner for 1 second then fades */
    public void ShowEnergyDrinkBanner()
    {
        if (energyDrinkBanner != null)
        {
            StartCoroutine(EnergyDrinkBannerSequence());
        }
    }

    /* Enables banner, waits, fades it out */
    private IEnumerator EnergyDrinkBannerSequence()
    {
        energyDrinkBanner.SetActive(true);
        yield return new WaitForSeconds(1f);

        CanvasGroup group = energyDrinkBanner.GetComponent<CanvasGroup>();
        if (group == null)
        {
            group = energyDrinkBanner.AddComponent<CanvasGroup>();
        }

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = 1f - (elapsed / duration);
            yield return null;
        }

        energyDrinkBanner.SetActive(false);
        group.alpha = 1f;
    }

    public void ShowDialogueHint()
    {
        if (dialogueHintText == null)
        {
            return;
        }

        if (dialogueHintCoroutine != null)
        {
            StopCoroutine(dialogueHintCoroutine);
        }

        Color color = dialogueHintText.color;
        color.a = 1f;
        dialogueHintText.color = color;
        dialogueHintText.gameObject.SetActive(true);

        dialogueHintCoroutine = StartCoroutine(FadeDialogueHint());
    }

    private IEnumerator FadeDialogueHint()
    {
        yield return new WaitForSecondsRealtime(1f);

        float duration = 0.5f;
        float elapsed = 0f;
        Color color = dialogueHintText.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / duration);
            dialogueHintText.color = color;
            yield return null;
        }

        dialogueHintText.gameObject.SetActive(false);
        color.a = 1f;
        dialogueHintText.color = color;
        dialogueHintCoroutine = null;
    }

    /* Fades popup text */
    private IEnumerator FadePopup(TMP_Text popup, bool isMoneyPopup)
    {
        float duration = 3f;
        float elapsed = 0f;
        Color color = popup.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / duration);
            popup.color = color;
            yield return null;
        }

        popup.gameObject.SetActive(false);
        color.a = 1f;
        popup.color = color;

        if (isMoneyPopup)
        {
            moneyPopupCoroutine = null;
        }
        else
        {
            qualityPopupCoroutine = null;
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void ShowUntimedTimer()
    {
        if (timerText == null)
        {
            return;
        }

        if (breatheCoroutine != null)
        {
            StopCoroutine(breatheCoroutine);
            breatheCoroutine = null;
        }

        timerText.color = Color.white;
        timerText.fontSize = 28f;
        timerText.text = "Time: --:---";
        timerText.gameObject.SetActive(true);
    }

    public void ShowEmptyFoodQuality()
    {
        if (foodQualityText != null)
        {
            foodQualityText.gameObject.SetActive(true);
            foodQualityText.text = "Quality: ---%";
        }
    }
}
