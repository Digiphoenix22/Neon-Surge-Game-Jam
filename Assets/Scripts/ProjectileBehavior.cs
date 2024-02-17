using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage = 1; // Customizable damage value for each projectile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile collided with an enemy
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Optionally destroy the projectile on impact
        }
}
}
