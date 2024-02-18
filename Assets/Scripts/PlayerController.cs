using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float acceleration = 10f;
    public float currentSpeed; // Ensure this declaration exists

    public float baseSpeed = 5f; // Base walking speed
    public float maxSpeed = 5f; // Max speed for bhopping
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5f;
    public float bhopMultiplier = 1.1f;
    private float currentSpeedMultiplier = 1f;
    private bool isGrounded;
    public bool isSprinting;
    public bool isCrouching = false;
    public float crouchMultiplier = 0.5f;

    [Header("Player Health")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Audio")]
    public AudioClip sonicBoomReadyClip;
    public AudioClip jumpSound;
    private AudioSource audioSource;

    [Header("UI")]
    public Image flashOverlay; // Optional for effects like sonic boom
    public Rigidbody2D rb; // Readonly property for external access
    private float movementInput;
    private bool sonicBoomAvailable = true;
    private float sonicBoomCooldown = 30f;
    private float lastSonicBoomTime = -30f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();
        HandleSonicBoom();
    }

    void FixedUpdate()
    {
        HandleMovement();
        currentSpeed = rb.velocity.magnitude;
    }

    void HandleInput()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
        movementInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
        currentSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f;
        if (Input.GetKeyDown(KeyCode.K)) OnPlayerDeath(); // Debug death
    }

    void HandleMovement()
    {
        if (!isCrouching) MovePlayer(movementInput);
        else ApplyMomentum(movementInput);
    }

    void MovePlayer(float direction)
    {
        rb.velocity = new Vector2(direction * baseSpeed * currentSpeedMultiplier, rb.velocity.y);
    }

    void ApplyMomentum(float direction)
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector2(direction * acceleration, 0) * crouchMultiplier, ForceMode2D.Force);
        }
    }

    void Jump()
    {
        float jumpStrength = isCrouching ? jumpForce * 1.1f : jumpForce;
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpSound);
    }

    void HandleSonicBoom()
    {
        if (Time.time - lastSonicBoomTime >= sonicBoomCooldown && currentSpeed > 30 && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SupersonicBoom());
            lastSonicBoomTime = Time.time;
        }
    }

    IEnumerator SupersonicBoom()
    {
        // Sonic boom logic here
        yield return new WaitForSeconds(0.1f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    public void OnPlayerDeath()
    {
        FindObjectOfType<DeathScreenController>().TriggerDeathScreen();
        this.enabled = false; // Disable this script to stop player control
    }
}
