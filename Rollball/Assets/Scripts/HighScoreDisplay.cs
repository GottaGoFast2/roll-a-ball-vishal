using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI bestTimeText;

    void Awake()
    {
        if (bestTimeText == null)
        {
            bestTimeText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
       // PlayerPrefs.DeleteKey("BestTime");
        UpdateBestTimeDisplay();
    }

    public void UpdateBestTimeDisplay()
    {
        if (bestTimeText != null)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime", 9999f);

            if (bestTime >= 9999f)
            {
                bestTimeText.text = "Best Time: --:--";
            }
            else
            {
                int minutes = Mathf.FloorToInt(bestTime / 60f);
                int seconds = Mathf.FloorToInt(bestTime % 60f);
                bestTimeText.text = string.Format("Best Time: {0:00}:{1:00}", minutes, seconds);
            }
        }
    }
}