using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

public class LevelTrigger : MonoBehaviour
{
    public string nextLevelName; // The name of the next level to load
    public GameObject winScreen; // Reference to the win screen UI element
    public CameraFollow cameraFollowScript; // Reference to the camera follow script

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
            
            // Freeze the camera by disabling its follow script
            if (cameraFollowScript) cameraFollowScript.enabled = false;
            
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
