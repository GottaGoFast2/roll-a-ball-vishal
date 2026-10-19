using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // 0 means start fresh
        PlayerPrefs.SetInt("LoadGameFlag", 0);
        SceneManager.LoadScene("MiniGame");
    }

    public void LoadGame()
    {
        // 1 means load the save state
        PlayerPrefs.SetInt("LoadGameFlag", 1);
        SceneManager.LoadScene("MiniGame");
    }

    public void OpenSettings()
    {
        Debug.Log("open settings");
    }

    public void ExitGame()
    {
        Debug.Log("exiting...");
        Application.Quit();
    }
}