using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText; // Drag your Timer Text UI here in Inspector
    public GameObject winTextObject;

    private Rigidbody rb;
    public int count;
    private float movementX;
    private float movementY;

    // Timer variables
    public float elapsedTime = 0f;
    private bool isTiming = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        winTextObject.SetActive(false);

        if (!PlayerPrefs.HasKey("IsLoadingTimer"))
        {
            elapsedTime = 0f;
        }
        else
        {
            PlayerPrefs.DeleteKey("IsLoadingTimer");
        }

        SetCountText();
    }

    void Update()
    {
        if (isTiming)
        {
            // Time.deltaTime automatically pauses when Time.timeScale = 0 (Pause Menu)
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Format time as Minutes:Seconds (e.g., 01:23)
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            isTiming = false; // Stop the stopwatch
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Victory!\nTime: " + timerText.text;
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

            // Debug log to check values
            Debug.Log("Win reached! elapsedTime value is: " + elapsedTime);

            float currentTime = elapsedTime;
            float currentBestTime = PlayerPrefs.GetFloat("BestTime", 9999f);

            Debug.Log("Comparing -> Current: " + currentTime + " | Best Saved: " + currentBestTime);

            if (currentTime > 0f && currentTime < currentBestTime)
            {
                PlayerPrefs.SetFloat("BestTime", currentTime);
                PlayerPrefs.Save();
                Debug.Log("SUCCESS: New best time saved!");
            }
            else
            {
                Debug.Log("FAILED TO SAVE: Either time was <= 0 or not faster than best.");
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            count = count + 1;
            other.gameObject.SetActive(false);
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isTiming = false;
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}