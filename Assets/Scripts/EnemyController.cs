using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    private Rigidbody2D enemyRB;

    [Header("Health Settings")]
    [SerializeField] private int currentHealth = 1;

    [Header("Movement Settings")]
    public float acceleration = 20f;
    public float maxSpeed = 10f;
    public float lockOnRadius = 5f;

    [Header("Combat Settings")]
    public float bounceForce = 2f; // Use this for bouncing off collisions
    public AudioClip HitSound;

    private AudioSource audioSource;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        enemyRB = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= lockOnRadius)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            enemyRB.AddForce(direction * acceleration);

            if (enemyRB.velocity.magnitude > maxSpeed)
            {
                enemyRB.velocity = enemyRB.velocity.normalized * maxSpeed;
            }
        }
    }

    public AudioClip deathSound; // Assign in the inspector

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Check if enemy is still alive to play the hit sound
        if(currentHealth > 0)
        {
            if(audioSource && HitSound)
            {
                audioSource.PlayOneShot(HitSound);
            }
        }
        else
        {
            // Enemy dies, play death sound instead
            if(audioSource && deathSound)
            {
                audioSource.PlayOneShot(deathSound);
                // Consider delaying the destruction if you want the death sound to play fully
                Destroy(gameObject, deathSound.length);
            }
            else
            {
                // If there's no death sound or audio source, destroy immediately
                Destroy(gameObject);
            }
        }
}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.TakeDamage(1);

            Vector2 direction = transform.position - collision.transform.position;
            enemyRB.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);

            enemyRB.velocity *= 0.5f;
        }
        else if (collision.CompareTag("Projectile"))
        {
            
            TakeDamage(1);
        }
    }

    // Respond to collisions with ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector2 bounceDirection = collision.contacts[0].normal; // Get the normal of the collision point
            enemyRB.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse); // Apply force in the bounce direction
        }
    }
}
