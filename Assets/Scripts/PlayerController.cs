using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float baseMoveSpeed = 5f; // Add a base speed for reset purposes
    public float jumpForce = 15f;
    public float speedBoostAmount = 1.5f;
    public int maxSpeedBoosts = 5;
    private int currentSpeedBoosts = 0;
    private Rigidbody2D rb;
    private bool isGrounded;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    private float lastJumpTime;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float bhopWindow = 0.2f; // Window of time to hit jump again for bhop
    private float lastGroundedTime; // Track the last time player was grounded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        Move();
        // Adjust jump check to consider last grounded time within bhop window
        if (Input.GetButtonDown("Jump") && (Time.time - lastGroundedTime <= bhopWindow))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Ground check moved to FixedUpdate for physics accuracy
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Ground"));
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            if (currentSpeedBoosts > 0 && Time.time - lastJumpTime > bhopWindow)
            {
                // Player missed the bhop window; reset speed boost count but keep the current speed
                currentSpeedBoosts = 0;
            }
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Use velocity change for more consistent jump
        if (currentSpeedBoosts < maxSpeedBoosts && isGrounded)
        {
            moveSpeed = Mathf.Min(moveSpeed + speedBoostAmount, baseMoveSpeed + speedBoostAmount * maxSpeedBoosts);
            currentSpeedBoosts++;
            audioSource.pitch = 1.0f + 0.1f * currentSpeedBoosts;
            audioSource.PlayOneShot(jumpSound);
            UpdateVisualCue();
        }
        lastJumpTime = Time.time; // Update the last jump time
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            spriteRenderer.color = originalColor; // Ensure color reset happens here
        }
    }

    void UpdateVisualCue()
    {
        float intensity = 0.1f * currentSpeedBoosts;
        spriteRenderer.color = Color.Lerp(originalColor, Color.blue, intensity);
    }
}