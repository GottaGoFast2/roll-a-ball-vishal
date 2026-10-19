using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenuUI;

    [Header("Player & Game Data")]
    public Transform playerTransform;         // Drag your Player here
    public PlayerController playerController; // Drag your Player here too

    public static bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Check if we clicked "Load Game" from the Main Menu
        if (PlayerPrefs.GetInt("LoadGameFlag", 0) == 1)
        {
            LoadGameData();
            // Reset the flag back to 0 so normal restarts don't auto-load
            PlayerPrefs.SetInt("LoadGameFlag", 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- SAVE GAME ---
    public void SaveGame()
    {
        if (playerTransform != null)
        {
            PlayerPrefs.SetFloat("PlayerX", playerTransform.position.x);
            PlayerPrefs.SetFloat("PlayerY", playerTransform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", playerTransform.position.z);
        }

        // Save the score counter (assuming your variable in PlayerController is named 'count')
        if (playerController != null)
        {
            PlayerPrefs.SetInt("SavedScore", playerController.count);
        }

        PlayerPrefs.Save();
        Debug.Log("Game Saved: Position & Score stored!");
    }

    // --- LOAD GAME ---
    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            Time.timeScale = 1f;
            isPaused = false;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

            // 1. Restore Position
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            if (playerTransform != null)
            {
                playerTransform.position = new Vector3(x, y, z);

                // Snap Camera using your camera's offset (X:0, Y:10, Z:-10)
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    mainCamera.transform.position = new Vector3(x, y + 10f, z - 10f);
                }
            }

            // 2. Restore Score Counter & UI Text
            if (playerController != null && PlayerPrefs.HasKey("SavedScore"))
            {
                int savedScore = PlayerPrefs.GetInt("SavedScore");
                playerController.count = savedScore;
                playerController.SetCountText();

                // 3. Despawn the pick-ups that were already collected!
                GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
                for (int i = 0; i < savedScore && i < pickups.Length; i++)
                {
                    pickups[i].SetActive(false);
                }
            }

            Debug.Log("Game Loaded Successfully!");
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}