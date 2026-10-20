using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText; // Drag your Volume Value Text here in Inspector

    void Start()
    {
        //  clear any stuck 0 values
        if (!PlayerPrefs.HasKey("VolumeSetting"))
        {
            PlayerPrefs.SetFloat("VolumeSetting", 100f);
        }

        float savedVolume = PlayerPrefs.GetFloat("VolumeSetting", 100f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 100f;
            volumeSlider.wholeNumbers = true;
            volumeSlider.value = savedVolume;
        }

        UpdateVolumeText(savedVolume);
        AudioListener.volume = savedVolume / 100f;
    }

    public void SetVolume(float sliderValue)
    {
        // Save the 0-100 value to PlayerPrefs
        PlayerPrefs.SetFloat("VolumeSetting", sliderValue);
        PlayerPrefs.Save();

        // Update the visual text number and the actual audio
        UpdateVolumeText(sliderValue);
        AudioListener.volume = sliderValue / 100f;
    }

    void UpdateVolumeText(float value)
    {
        if (volumeValueText != null)
        {
            volumeValueText.text = value.ToString("0"); // Displays as a clean integer (e.g., "75")
        }
    }
}