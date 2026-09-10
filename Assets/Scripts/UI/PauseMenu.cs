using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject howToPlayPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private float buttonSoundVolume = 0.7f;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;

        pauseOverlay.SetActive(false);
        pausePanel.SetActive(true);
        howToPlayPanel.SetActive(false);

        if (uiAudioSource != null)
        {
            uiAudioSource.ignoreListenerPause = true;
        }

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        PlayButtonSound();
        AudioListener.pause = true;

        isPaused = true;

        pauseOverlay.SetActive(true);
        pausePanel.SetActive(true);
        howToPlayPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        PlayButtonSound();

        isPaused = false;

        pauseOverlay.SetActive(false);
        pausePanel.SetActive(true);
        howToPlayPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ShowHowToPlay()
    {
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        howToPlayPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void PlayButtonSound()
    {
        if (uiAudioSource != null && buttonClickSound != null)
        {
            uiAudioSource.PlayOneShot(
                buttonClickSound,
                buttonSoundVolume
            );
        }
    }

    private void OnDisable()
    {
        AudioListener.pause = false;

        Time.timeScale = 1f;
    }
}
