using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float acceleration = 10f;
    public int maxHealth = 10;
    private int currentHealth;
    public float maxSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5f;
    public float bhopMultiplier = 1.1f;
    private float currentSpeedMultiplier = 1f;
    private bool isGrounded;
    public bool isSprinting; // Flag to indicate sprinting state
    public float currentSpeed; // Variable to store the current speed

    public AudioClip sonicBoomReadyClip; // Assign this in the inspector
    public AudioClip jumpSound; // Assign this in the inspector

    private AudioSource audioSource; // Assign or add an AudioSource component and assign this in Start()
    private bool sonicBoomAvailable = true;
    private float sonicBoomCooldown = 30f;
    private float lastSonicBoomTime = -30f; // Initialize to -cooldown so ability is available at start

    public Image flashOverlay; // Assign this in the inspector

    public Rigidbody2D rb;
    private float movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        // Get horizontal movement input
        movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            audioSource.PlayOneShot(jumpSound);
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
        // DEBUG BUTTON
        if (Input.GetKeyDown(KeyCode.K))
        {
        OnPlayerDeath();
        }




        // sonic boom

         // Play the sound queue if the cooldown is over, the player is over the speed of 30, and the sound has not yet been played
         // Check if we can play the Sonic Boom ready sound queue
        if (Time.time - lastSonicBoomTime >= sonicBoomCooldown && currentSpeed > 30)
        {
        if (!sonicBoomAvailable)
        {
            audioSource.PlayOneShot(sonicBoomReadyClip);
            sonicBoomAvailable = true; // Prevent the sound from playing repeatedly
        }
        // Check for Sonic Boom input (e.g., pressing "S" key)
        if (Input.GetKeyDown(KeyCode.F) && currentSpeed > 30 && Time.time - lastSonicBoomTime >= sonicBoomCooldown)
        {
            StartCoroutine(SupersonicBoom());
            lastSonicBoomTime = Time.time;
            sonicBoomAvailable = true; // The ability can be signaled as ready again after use
        }
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
    // Assume you have a method to invert colors on the screen
    //InvertScreenColors(true);
    KillAllEnemies();
    yield return new WaitForSeconds(0.1f);
    //InvertScreenColors(false);

    // Start cooldown (handled in Update)
    }


     void KillAllEnemies()
    {
        // Find all enemies in the scene and destroy them
        //foreach (var enemy in FindObjectsOfType<Enemy>()) // Assuming your enemies have an "Enemy" script
        {
       //     Destroy(enemy.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentHealth -= 1;
    }
    public void OnPlayerDeath()
    {
    // Assuming you have a reference to the DeathScreenController
    FindObjectOfType<DeathScreenController>().TriggerDeathScreen();
    
    // Disable player controls
    GetComponent<PlayerController>().enabled = false; // Or any other way you handle disabling controls
    }




}
