using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Make sure Main Menu shows and Settings stays hidden on launch
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Audio Manager")]
    public AudioManager audioManager;

    public void StartGame()
    {
        PlayerPrefs.SetInt("LoadGameFlag", 0);
        SceneManager.LoadScene("MiniGame");
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("LoadGameFlag", 1);
        SceneManager.LoadScene("MiniGame");
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        // Switch to Settings music
        if (audioManager != null) audioManager.PlaySettingsMusic();
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // Switch back to Main Menu music
        if (audioManager != null) audioManager.PlayMainMenuMusic();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}