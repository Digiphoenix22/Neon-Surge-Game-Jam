using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections; 


public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public CanvasGroup menuCanvasGroup; // CanvasGroup to fade in
    public float fadeInDuration = 2f; // Duration of the fade-in effect

    void Start()
    {
        // Start with the menu faded out and not interactable
        menuCanvasGroup.alpha = 0;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        StartCoroutine(FadeInMenu());

        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    IEnumerator FadeInMenu()
    {
        // Wait for 10 seconds before starting the fade in
        yield return new WaitForSeconds(8f);

        float timer = 0;
        while (timer <= fadeInDuration)
        {
            // Increment timer by the time between frames and calculate the new alpha
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeInDuration);

            // Apply the new alpha to the CanvasGroup
            menuCanvasGroup.alpha = alpha;

            yield return null; // Wait for the next frame
        }

        // Ensure everything is fully visible and interactable after fading in
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }
    

    void PlayGame()
    {
        SceneManager.LoadScene("Level1"); // Replace "GameScene" with your game scene's name
    }

    void OpenOptions()
    {
        // Load options scene or display options panel
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
