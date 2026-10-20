using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Make sure this is here so we can reference Button

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenuUI;
    public Button saveButton; // Drag your Save Game Button here in Inspector

    [Header("Player & Game Data")]
    public Transform playerTransform;         
    public PlayerController playerController; 

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

        // Check if the game is already won/completed. If so, disable the Save button!
        if (playerController != null && playerController.winTextObject != null)
        {
            bool isGameOver = playerController.winTextObject.activeSelf;
            if (saveButton != null)
            {
                saveButton.interactable = !isGameOver; // Disables button and automatically fades it visually if using standard Unity UI
            }
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- SAVE GAME ---
    public void SaveGameData()
    {
        // Prevent saving if the game is already won/completed
        if (playerController != null && playerController.winTextObject != null && playerController.winTextObject.activeSelf)
        {
            Debug.LogWarning("You cannot save a completed game!");
            return;
        }

        if (playerTransform != null && playerController != null)
        {
            PlayerPrefs.SetFloat("PlayerX", playerTransform.position.x);
            PlayerPrefs.SetFloat("PlayerY", playerTransform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", playerTransform.position.z);
            PlayerPrefs.SetInt("SavedScore", playerController.count);

            // Save the current stopwatch time!
            PlayerPrefs.SetFloat("SavedTime", playerController.elapsedTime);

            PlayerPrefs.Save();
            Debug.Log("Game Saved Successfully! Time saved: " + playerController.elapsedTime);
        }
    }

    // --- LOAD GAME ---
    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            Time.timeScale = 1f;
            isPaused = false;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

            // 1. Restore Position & Camera
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            if (playerTransform != null)
            {
                playerTransform.position = new Vector3(x, y, z);
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    mainCamera.transform.position = new Vector3(x, y + 10f, z - 10f);
                }
            }

            // 2. Restore Score, Pick-ups, and Timer
            if (playerController != null)
            {
                if (PlayerPrefs.HasKey("SavedScore"))
                {
                    int savedScore = PlayerPrefs.GetInt("SavedScore");
                    playerController.count = savedScore;
                    playerController.SetCountText();

                    // Despawn collected pick-ups
                    GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
                    for (int i = 0; i < savedScore && i < pickups.Length; i++)
                    {
                        pickups[i].SetActive(false);
                    }
                }

                // Restore the stopwatch time
                if (PlayerPrefs.HasKey("SavedTime"))
                {
                    playerController.elapsedTime = PlayerPrefs.GetFloat("SavedTime");
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