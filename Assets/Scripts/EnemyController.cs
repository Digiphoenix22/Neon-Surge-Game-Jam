using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    // Assuming you have a PlayerController script that you want to reference for some reason
    public PlayerController player; // You might not need this unless it's used for specific interactions
    private Rigidbody2D enemyRB;

    public float detectionRange = 5f; // Example variable for detection range, assuming you want to use it later

    void Start()
    {
    
        enemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Example usage of detectionRange, adjust as necessary
        // enemyRB.velocity = player.rb.velocity; // This line was directly copying the player's velocity to the enemy, which might not be what you want
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy or play death animation
        Destroy(gameObject);
    }
}
