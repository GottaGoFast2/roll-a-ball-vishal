using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource bgmSource;

    [Header("Audio Clips")]
    public AudioClip mainMenuClip;      // New Super Mario Bros DS
    public AudioClip settingsClip;      // Wii Sports Resort Settings
    public AudioClip gameplayClip;      // Wii Sports Tennis Training
    public AudioClip pauseClip;         // Artistic Expression

    void Awake()
    {
        // Automatically add or get AudioSource if not assigned
        if (bgmSource == null)
        {
            bgmSource = GetComponent<AudioSource>();
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
            }
        }

        bgmSource.loop = true; // Ensure music always loops
    }

    void Start()
    {
        // Default track when menu loads
        PlayMainMenuMusic();
    }

    public void PlayTrack(AudioClip clip)
    {
        if (clip == null) return;

        // Only swap if a new song is being requested
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayMainMenuMusic()
    {
        PlayTrack(mainMenuClip);
    }

    public void PlaySettingsMusic()
    {
        PlayTrack(settingsClip);
    }

    public void PlayGameplayMusic()
    {
        PlayTrack(gameplayClip);
    }

    public void PlayPauseMusic()
    {
        PlayTrack(pauseClip);
    }
}