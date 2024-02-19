using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float deceleration = 30f;
    public float acceleration = 10f;
    public float currentSpeed;
    public float baseSpeed = 5f;
    public float maxSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5f;
    private float currentSpeedMultiplier = 1.5f;
    private bool isGrounded;
    public bool isSprinting;
    public bool isCrouching = false;
    public float crouchMultiplier = 0.5f;

    [Header("Bhopping Properties")]
    public float bhopMultiplier = 1.1f;
    public AudioClip bhopJumpSound;
    private int consecutiveBhops = 0;
    public float pitchIncreaseFactor = 0.1f;
    public float bhopWindow = 0.3f;
    private float timeSinceLastBhop = 0f;
    public ParticleSystem bhopParticlePrefab; 

    [Header("Player Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Audio")]
    public AudioClip sonicBoomReadyClip;
    public AudioClip jumpSound;
    public AudioClip BhopJumpSound;
    private AudioSource audioSource;
    public AudioMixerGroup soundEffectsMixerGroup; 


    [Header("UI")]
    public Image flashOverlay;
    public Rigidbody2D rb;
    private float movementInput;
    private bool sonicBoomAvailable = true;
    private float sonicBoomCooldown = 30f;
    private float lastSonicBoomTime = -30f;

    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite movingSprite;
    public Sprite jumpingSpriqte;

    private SpriteRenderer spriteRenderer;

    [Header("Animation")]
    public Animator animator;

    [Header("Damage/Combat")]
    public bool ifDead;
    

    void Start()
    {
    
        rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = soundEffectsMixerGroup; // Assign the mixer group
        audioSource.pitch = 1.0f;
    }

    

    void Update()
    {
        HandleInput();
        HandleSonicBoom();
        HandleAnimation();
        
    }

    void FixedUpdate()
    {
        HandleMovement();
        currentSpeed = rb.velocity.magnitude;
    }

    void HandleMovement()
    {
        if (!isCrouching) 
        {
            MovePlayer(movementInput);
        }
        else
        {
            ApplyMomentum(movementInput);
        }

        if (isGrounded && timeSinceLastBhop > bhopWindow)
        {
            ResetBhopState();
        }
    }

    void MovePlayer(float direction)
    {
        float targetSpeed = direction * baseSpeed * currentSpeedMultiplier;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        rb.AddForce(Vector2.right * speedDiff * accelerationRate, ForceMode2D.Force);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -baseSpeed * currentSpeedMultiplier, baseSpeed * currentSpeedMultiplier), rb.velocity.y);
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
        float jumpStrength = jumpForce;
        if (isCrouching && isGrounded)
        {
            consecutiveBhops++;

            // Directly increase speed in the current direction
            Vector2 increaseSpeedDirection = rb.velocity.normalized * acceleration * bhopMultiplier;
            rb.velocity += increaseSpeedDirection; // Directly add to the current velocity

            PlayJumpSound(true);
        }
        else
        {
            PlayJumpSound(false);
        }

        // Apply jump force
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

        isGrounded = false;
        if(isGrounded) animator.SetBool("IsJumping", true);
    }


    void PlayJumpSound(bool isBhop)
    {
        float volume = 0.7f; 
        if (isBhop && bhopJumpSound)
        {
            audioSource.pitch = 1.0f + (pitchIncreaseFactor * Mathf.Min(consecutiveBhops, 5));
            audioSource.PlayOneShot(bhopJumpSound, volume);
        }
        else
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(jumpSound);
        }
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
        yield return new WaitForSeconds(0.1f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            timeSinceLastBhop = 0f;

            // Check if this landing should trigger the bhop particle effect
            if (consecutiveBhops > 1) // Assuming 1 means just a regular jump, adjust as needed
            {
                SpawnBhopParticles();
            }
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false); // Player has landed
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    void SpawnBhopParticles()
    {
        if (bhopParticlePrefab != null)
        {
            // Instantiate the particle system at the player's position
            ParticleSystem effect = Instantiate(bhopParticlePrefab, transform.position, Quaternion.identity);
            effect.Play();

            // Optionally, destroy the particle system once it has completed its emission
            Destroy(effect.gameObject, effect.main.duration);
        }
    }

    public void TakeDamage(int damage)
    {
    currentHealth -= damage;
    if (currentHealth <= 0)
    {
        OnPlayerDeath();
        ifDead = true;
    }
    }

    public void OnPlayerDeath()
    {
        FindObjectOfType<DeathScreenController>().TriggerDeathScreen();
        this.enabled = false;
        
    }

    void HandleInput()
    {
        bool wasCrouching = isCrouching;
        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
        movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
        currentSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f;
        if (Input.GetKeyDown(KeyCode.K)) OnPlayerDeath();

        if (wasCrouching && !isCrouching)
        {
            ResetBhopState(true);
        }
    }

    void ResetBhopState(bool dueToCrouchRelease = false)
    {
        if (dueToCrouchRelease || (isGrounded && timeSinceLastBhop > bhopWindow))
        {
            consecutiveBhops = 0;
            audioSource.pitch = 1.0f;
            currentSpeedMultiplier = 1f;
            timeSinceLastBhop = 0f;
        }
    }

    void HandleAnimation()
    {
        // Check for horizontal movement to trigger moving animation
        animator.SetBool("Moving", Mathf.Abs(movementInput) > 0);
    
    
    animator.SetBool("IsJumping", !isGrounded);
    }
}
