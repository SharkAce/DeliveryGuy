using System.Collections;
using TMPro;
using UnityEngine;

public class HUDDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private TMP_Text foodQualityText;
    [SerializeField] private GameObject energyDrinkBanner;
    [SerializeField] private TMP_Text dialogueHintText;

    private Coroutine breatheCoroutine;

    private void Start()
    {
        /* Find deactivated UI children by name*/
        if(energyDrinkBanner == null)
        {
            Transform banner = transform.Find("EnergyDrinkBanner");
            if(banner != null) energyDrinkBanner = banner.gameObject;
        }

        if(dialogueHintText == null)
        {
            Transform hint = transform.Find("DialogueHint");
            if(hint != null) dialogueHintText = hint.GetComponent<TMP_Text>();
        }
    }

    /* Update money display */
    public void UpdateMoney(float amount)
    {
        if(moneyText != null)
        {
            moneyText.text = "$" + amount.ToString("F0");
        }
    }

    /* Update food quality display */
    public void UpdateFoodQuality(float quality)
    {
        if(foodQualityText != null)
        {
            foodQualityText.text = "Quality: " + quality.ToString("F0") + "%";
        }
    }

    /* Shows red popup for money spent on energy drinks */
    public void ShowMoneySpentPopup(float amount)
    {
        if(popupText == null) return;
        StopAllCoroutines();
        popupText.text = "-$" + amount.ToString("F0");
        popupText.color = new Color(0.9f, 0.1f, 0.1f);
        popupText.gameObject.SetActive(true);
        StartCoroutine(FadePopup());
    }

    /* Update countdown timer, turns red and breathes when at zero */
    public void UpdateTimer(float remaining)
    {
        if(timerText == null) return;

        remaining = Mathf.Max(0f, remaining);

        int seconds = Mathf.FloorToInt(remaining);
        int milliseconds = Mathf.FloorToInt((remaining - seconds) * 1000f);
        timerText.text = "Time: " + seconds.ToString("00") + ":" + milliseconds.ToString("000");

        if(remaining <= 5f)
        {
            timerText.color = new Color(0.8f, 0f, 0f);
            if(breatheCoroutine == null)
            {
                breatheCoroutine = StartCoroutine(BreatheTimer());
            }
        }
        else
        {
            timerText.color = Color.white;
            if(breatheCoroutine != null)
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

    /* Hides timer between deliveries */
    public void HideTimer()
    {
        if(timerText != null)
        {
            if(breatheCoroutine != null)
            {
                StopCoroutine(breatheCoroutine);
                breatheCoroutine = null;
            }
            timerText.gameObject.SetActive(false);
        }
    }

    /* Shows timer when delivery begins */
    public void ShowTimer()
    {
        if(timerText != null)
        {
            timerText.color = Color.white;
            timerText.gameObject.SetActive(true);
        }
    }

    /* Shows green popup for tips earned */
    public void ShowPositivePopup(float amount)
    {
        if(popupText == null) return;
        StopAllCoroutines();
        popupText.text = "+$" + amount.ToString("F0");
        popupText.color = new Color(0.2f, 0.8f, 0.2f);
        popupText.gameObject.SetActive(true);
        StartCoroutine(FadePopup());
    }

    /* Shows red popup for quality penalty from crashes */
    public void ShowNegativePopup(float amount)
    {
        if(popupText == null) return;
        StopAllCoroutines();
        popupText.text = "-" + amount.ToString("F0") + "% quality";
        popupText.color = new Color(0.9f, 0.1f, 0.1f);
        popupText.gameObject.SetActive(true);
        StartCoroutine(FadePopup());
    }

    /* Shows energy drink banner for 1 second then fades */
    public void ShowEnergyDrinkBanner()
    {
        if(energyDrinkBanner != null)
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
        if(group == null)
        {
            group = energyDrinkBanner.AddComponent<CanvasGroup>();
        }

        float duration = 0.5f;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = 1f - (elapsed / duration);
            yield return null;
        }

        energyDrinkBanner.SetActive(false);
        group.alpha = 1f;
    }

    /* Shows dialogue hint when car is locked */
    public void ShowDialogueHint()
    {
        if(dialogueHintText != null)
        {
            dialogueHintText.gameObject.SetActive(true);
        }
    }

    /* Hides dialogue hint when dialogue ends */
    public void HideDialogueHint()
    {
        if(dialogueHintText != null)
        {
            dialogueHintText.gameObject.SetActive(false);
        }
    }

    /* Fades popup text */
    private IEnumerator FadePopup()
    {
        float duration = 3f;
        float elapsed = 0f;
        Color color = popupText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / duration);
            popupText.color = color;
            yield return null;
        }

        popupText.gameObject.SetActive(false);
        color.a = 1f;
        popupText.color = color;
    }

    /* Shows a custom message popup in red */
    public void ShowMessagePopup(string message)
    {
        if(popupText == null) return;
        StopAllCoroutines();
        popupText.text = message;
        popupText.color = new Color(0.9f, 0.1f, 0.1f);
        popupText.gameObject.SetActive(true);
        StartCoroutine(FadePopup());
    }
}