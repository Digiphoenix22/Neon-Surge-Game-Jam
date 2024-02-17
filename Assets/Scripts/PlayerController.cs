using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5f;
    public float bhopMultiplier = 1.1f;
    private float currentSpeedMultiplier = 1f;
    private bool isGrounded;
    public bool isSprinting; // Flag to indicate sprinting state
    public float currentSpeed; // Variable to store the current speed

    public AudioClip sonicBoomReadyClip; // Assign this in the inspector
    private AudioSource audioSource; // Assign or add an AudioSource component and assign this in Start()
    private bool sonicBoomAvailable = true;
    private float sonicBoomCooldown = 30f;
    private float lastSonicBoomTime = -30f; // Initialize to -cooldown so ability is available at start


    private Rigidbody2D rb;
    private float movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        // Get horizontal movement input
        movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeedMultiplier = sprintMultiplier;
        }
        else
        {
            currentSpeedMultiplier = 1f;
        }
        
        Sprint(Input.GetKey(KeyCode.LeftShift));

        // Reset speed multiplier when grounded without jumping
        if (isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            ResetSpeedMultiplier();
        }

        // Reset speed multiplier when grounded without jumping
        if (isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            currentSpeedMultiplier = 1f;
        }

         // Play the sound queue if the cooldown is over, the player is over the speed of 30, and the sound has not yet been played
        if (Time.time - lastSonicBoomTime >= sonicBoomCooldown && currentSpeed > 30 && sonicBoomAvailable)
        {
            audioSource.PlayOneShot(sonicBoomReadyClip);
            sonicBoomAvailable = false; // Prevent the sound from playing repeatedly
        }

        // Check for Sonic Boom input (e.g., pressing "S" key)
        if (Input.GetKeyDown(KeyCode.S) && currentSpeed > 30 && Time.time - lastSonicBoomTime >= sonicBoomCooldown)
        {
            StartCoroutine(SupersonicBoom());
            lastSonicBoomTime = Time.time;
            sonicBoomAvailable = true; // The ability can be signaled as ready again after use
        }
    }

    void FixedUpdate()
    {
        MovePlayer(movementInput);
        currentSpeed = rb.velocity.magnitude; // Update the current speed
    }

    void MovePlayer(float direction)
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed * currentSpeedMultiplier)
        {
            rb.AddForce(new Vector2(direction * acceleration, 0f), ForceMode2D.Force);
        }

        // Implement momentum by limiting speed only when above maxSpeed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed * currentSpeedMultiplier)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed * currentSpeedMultiplier, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
        if (currentSpeedMultiplier > 1f)
        {
            // Increase speed multiplier on successful bhop
            currentSpeedMultiplier *= bhopMultiplier;
            // Ensure the speed multiplier doesn't exceed a maximum threshold to prevent unlimited acceleration
            currentSpeedMultiplier = Mathf.Min(currentSpeedMultiplier, sprintMultiplier * 2);
        }
        else
        {
            // Initial speed boost on first jump
            currentSpeedMultiplier = sprintMultiplier;
        }
        ResetSpeedMultiplier();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void Sprint(bool isSprinting)
    {
        this.isSprinting = isSprinting; // Update the flag

        if (isSprinting && isGrounded)
        {
            currentSpeedMultiplier = sprintMultiplier;
        }
        else
        {
            ResetSpeedMultiplier();
        }
    }

    void ResetSpeedMultiplier()
    {
        currentSpeedMultiplier = 1f;
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnGUI()
    {
        // Display a label for sprinting status
        GUI.Label(new Rect(10, 10, 200, 20), "Is Sprinting: " + isSprinting);

        // Display a label for current speed
        GUI.Label(new Rect(10, 30, 200, 20), "Current Speed: " + currentSpeed.ToString("F2"));
    }

    IEnumerator SupersonicBoom()
    {
        // Invert colors for 0.1 seconds
        Camera.main.gameObject.GetComponent<CameraFilterPack_Colors_Adjust_ColorRGB>().enabled = true;
        KillAllEnemies();
        yield return new WaitForSeconds(0.1f);
        Camera.main.gameObject.GetComponent<CameraFilterPack_Colors_Adjust_ColorRGB>().enabled = false;

        // Start cooldown (handled in Update)
    }

     void KillAllEnemies()
    {
        // Find all enemies in the scene and destroy them
        foreach (var enemy in FindObjectsOfType<Enemy>()) // Assuming your enemies have an "Enemy" script
        {
            Destroy(enemy.gameObject);
        }
    }



}
