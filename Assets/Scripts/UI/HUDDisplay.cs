using System.Collections;
using TMPro;
using UnityEngine;

public class HUDDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text popupText;

    /* Update money display*/
    public void UpdateMoney(float amount)
    {
        if(moneyText != null)
        {
            moneyText.text = "$" + amount.ToString("F0");
        }
    }

    /* Update timer display, turns red when over targer time*/
    public void UpdateTimer(float time, bool overTime = false)
    {
        if(timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            timerText.color = overTime ? Color.red : Color.white;
        }
    }

    /* Hides timer between deliveries*/
    public void HideTimer()
    {
        if(timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    /* Show timer when delivery begins*/
    public void ShowTimer()
    {
        if(timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
    }

    /* Show quick money +- popup*/
    public void ShowPopup(float amount)
    {
        if(popupText == null) return;

        StopAllCoroutines();
        string prefix = amount >= 0 ? "+": "";
        popupText.text = prefix + "$" + amount.ToString("F0");
        popupText.gameObject.SetActive(true);
        StartCoroutine(FadePopup());
    }

    /*Fade popup text*/
    private IEnumerator FadePopup()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Color color = popupText.color;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed/duration);
            popupText.color = color;
            yield return null;
        }

        popupText.gameObject.SetActive(false);
        color.a = 1f;
        popupText.color = color;
    }
}