using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreenController : MonoBehaviour
{
    public TextMeshProUGUI restartText; 

    public PlayerController playerController;


    public float fadeInTime = 2f;
    private Image deathScreenImage;
    private bool isDead = false;

    public Text deathMessageText;


    void Start()
    {
        deathScreenImage = GetComponent<Image>();
        deathScreenImage.color = new Color(deathScreenImage.color.r, deathScreenImage.color.g, deathScreenImage.color.b, 0);
        playerController = GetComponent<PlayerController>();

    }

    void Update()
    {
        if (isDead)
        {
            
        }
    }

    public void TriggerDeathScreen()
    {
    isDead = true;
    StartCoroutine(FadeInDeathScreen());
    StartCoroutine(FadeInDeathMessage()); 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

IEnumerator FadeInDeathScreen()
    {
    float targetAlpha = 0.7f; // Target alpha value
    float timer = 0;
    while (timer <= fadeInTime)
        {
        float alphaChange = Mathf.Lerp(0, targetAlpha, timer / fadeInTime);
        deathScreenImage.color = new Color(deathScreenImage.color.r, deathScreenImage.color.g, deathScreenImage.color.b, alphaChange);
        timer += Time.deltaTime;
        yield return null;
        }
    }
    
    IEnumerator FadeInDeathMessage()
    {
        deathMessageText.color = new Color(deathMessageText.color.r, deathMessageText.color.g, deathMessageText.color.b, 0); // Start transparent
        float timer = 0;
        while (timer <= fadeInTime)
        {
            float alphaChange = Mathf.Lerp(0, 1, timer / fadeInTime); // Fade to fully visible
            deathMessageText.color = new Color(deathMessageText.color.r, deathMessageText.color.g, deathMessageText.color.b, alphaChange);
            timer += Time.deltaTime;
            yield return null;
        }
        if (restartText != null)
        {
        restartText.color = new Color(restartText.color.r, restartText.color.g, restartText.color.b, 1f);
        }
    }
}
