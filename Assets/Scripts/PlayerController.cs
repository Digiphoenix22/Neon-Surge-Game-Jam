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

    private Rigidbody2D rb;
    private float movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground"));

        // Get horizontal movement input
        movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
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

        // Reset speed multiplier when grounded without jumping
        if (isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            currentSpeedMultiplier = 1f;
        }
    }

    void FixedUpdate()
    {
        MovePlayer(movementInput);
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
    }
}
