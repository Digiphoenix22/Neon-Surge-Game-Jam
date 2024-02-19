using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections; 
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public CanvasGroup menuCanvasGroup; // CanvasGroup to fade in
    public float fadeInDuration = 2f; // Duration of the fade-in effect
    public AudioClip hoverSound;
    public AudioClip confirmSound;
    private AudioSource audioSource;
    public GameObject controlsInstructions;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Make sure there's an AudioSource component attached to the same GameObject
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start with the menu faded out and not interactable
        menuCanvasGroup.alpha = 0;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        StartCoroutine(FadeInMenu());

        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);

        // Add event triggers for hover sound
        AddEventTrigger(playButton, EventTriggerType.PointerEnter, () => PlaySound(hoverSound));
        AddEventTrigger(optionsButton, EventTriggerType.PointerEnter, () => PlaySound(hoverSound));
        AddEventTrigger(quitButton, EventTriggerType.PointerEnter, () => PlaySound(hoverSound));

        // Optionally, you could also add a sound for clicking (confirming) if different from the SceneManager load
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && controlsInstructions.activeSelf)
        {
            controlsInstructions.SetActive(false);
        }
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
        PlaySound(confirmSound);
        SceneManager.LoadScene("Level1"); // Replace "Level1" with your game scene's name
    }

    void OpenOptions()
    {
        PlaySound(confirmSound);
        controlsInstructions.SetActive(!controlsInstructions.activeSelf);
        // Load options scene or display options panel
    }

    void QuitGame()
    {
        PlaySound(confirmSound);
        Application.Quit();
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void AddEventTrigger(Button button, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }
}