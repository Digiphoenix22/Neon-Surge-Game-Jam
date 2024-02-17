using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    public float floatDistance = 2f; // Distance from the player at which the shotgun floats
    public float floatSpeed = 5f; // Speed at which the shotgun adjusts its position
    public Transform firePoint; // The point from where the bullets are shot, should be a child of the player GameObject
    public GameObject projectilePrefab; // The projectile prefab
    public float projectileForce = 20f; // The force at which the projectile is shot
    public int pelletsCount = 10; // Number of pellets per shot for the shotgun
    public float spreadAngle = 45f; // Spread angle of the shotgun blast

    void Update()
    {
        AimAndFloatTowardsCursor();

    if (Input.GetMouseButtonDown(0)) // Left mouse button
    {
        Shoot();
    }
    }


    void AimAndFloatTowardsCursor()
    {
    // Assuming this script is attached to the Shotgun GameObject
    Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    void Shoot()
    {
        for (int i = 0; i < pelletsCount; i++)
        {
            // Calculate spread for each pellet
            float spread = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Quaternion pelletRotation = firePoint.rotation * Quaternion.Euler(0, 0, spread);
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, pelletRotation);
            
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(projectile.transform.right * projectileForce, ForceMode2D.Impulse);
            }
        }
    }
}
