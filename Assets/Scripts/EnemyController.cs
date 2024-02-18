using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public GameObject Player;
    private PlayerController playerController; 
    private Rigidbody2D enemyRB;

    private float detectionTime;

    public float detectionRange = 5f; 

    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        enemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (detection(transform, Player.transform, detectionRange))
        {
            // Do something when objects are in range
            Debug.Log("Objects are in range!");
            enemyRB.velocity = -playerController.rb.velocity;
        }
        setSpeed();
    }

    public void takeDamage(int damage)
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

    bool detection(Transform thisTransform, Transform otherTransform, float range)
    {
        float distance = Vector3.Distance(thisTransform.position, otherTransform.position);
        return distance <= range;
    }

    void setSpeed()
    {
        if (detection(transform, Player.transform, detectionRange))
        {
            enemyRB.velocity = -playerController.rb.velocity;
        }
        else
        {
            enemyRB.velocity /= 2 + 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentHealth -= 1;
    }
}
