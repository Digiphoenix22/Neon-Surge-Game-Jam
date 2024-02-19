using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text speedDisplay;
    public TMP_Text healthDisplay;

    public PlayerController playerController; 

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerController != null)
        {
            // Update speed display
            speedDisplay.text = "Speed: " + playerController.currentSpeed.ToString("F2");

            // Update health display
            healthDisplay.text = "Health: " + playerController.currentHealth.ToString();
        }
    }
}

