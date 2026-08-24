using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject howToPlayPanel;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;

        pauseOverlay.SetActive(false);
        pausePanel.SetActive(true);
        howToPlayPanel.SetActive(false);

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
        isPaused = true;

        pauseOverlay.SetActive(true);
        pausePanel.SetActive(true);
        howToPlayPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
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

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}