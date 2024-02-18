using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioClip backgroundMusicClip; // Assign this in the inspector
    private AudioSource audioSource;

    public AudioMixerGroup musicMixerGroup;

    private string mainMenuSceneName = "MainMenu"; // Set this to your Main Menu scene name

    void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundMusicClip;
            audioSource.loop = true; // Loop the music

            audioSource.outputAudioMixerGroup = musicMixerGroup; // Set the output Audio Mixer Group

            DontDestroyOnLoad(gameObject); // Keep the music playing between scenes
            PlayMusic();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the Main Menu and stop the music if it is
        if (scene.name == mainMenuSceneName)
        {
            StopMusic();
        }
        else
        {
            // Otherwise ensure the music is playing
            PlayMusic();
        }
    }

    void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
