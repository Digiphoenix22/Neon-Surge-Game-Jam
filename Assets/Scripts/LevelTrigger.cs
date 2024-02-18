using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; // For loading scenes

public class LevelTrigger : MonoBehaviour
{
    public string nextLevelName; // The name of the next level to load
    public GameObject winScreen; // Reference to the win screen UI element

    private void Start()
    {
        // Ensure the win screen is not visible at the start
        if (winScreen) winScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure to tag your player GameObject with "Player"
        {
            // Show the win screen
            if (winScreen) winScreen.SetActive(true);
            
            // Optionally wait for a few seconds or wait for player input before loading the next level
            // For simplicity, here we'll just load the next level directly
            // Consider using a coroutine if you want a delay
            Invoke("LoadNextLevel", 2f); // Adjust the delay as needed
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
