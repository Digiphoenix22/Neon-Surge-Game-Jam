using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public GameObject Player;
    private PlayerController playerController;
    private Rigidbody2D enemyRB;

    public float detectionRange = 5f;
    public Vector2 followSpeed; // Customizable speed for following the player

    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        enemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Check if player is within detection range
        if (IsPlayerInRange())
        {
            // Move towards the player using the follow speed
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            enemyRB.velocity = (direction * (playerController.rb.velocity * 2));
        }
        else
        {
            // If player is not in range, stop moving
            enemyRB.velocity = Vector2.zero;
        }
    }

    bool IsPlayerInRange()
    {
        // Check if the distance between enemy and player is within the detection range
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        return distance <= detectionRange;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Reduce health if collided with another object (e.g., a projectile)
        currentHealth -= 1;
    }
}
