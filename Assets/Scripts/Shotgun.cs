using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    public float cockTime = 0.3f; // Time in seconds to wait before the next shot
    public GameObject shellPrefab; // Prefab for the shotgun shell to be spawned
    public AudioClip reloadSound; // Sound to play for reloading
    private bool isReloading = false; // Flag to check if currently reloading

    public float floatDistance = 2f; // Distance from the player at which the shotgun floats
    public float floatSpeed = 5f; // Speed at which the shotgun adjusts its position
    public Transform firePoint; // The point from where the bullets are shot, should be a child of the player GameObject
    public GameObject projectilePrefab; // The projectile prefab
    public float projectileForce = 20f; // The force at which the projectile is shot
    public int pelletsCount = 10; // Number of pellets per shot for the shotgun
    public float spreadAngle = 45f; // Spread angle of the shotgun blast
    public Transform playerTransform;
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
    Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 direction = cursorPosition - (Vector2)transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    // Calculate the desired floating position
    Vector2 playerPosition = playerTransform.position; // Ensure you have a reference to the player's transform
    Vector2 floatPosition = playerPosition + direction.normalized * floatDistance;

    // Smoothly move the shotgun to the floating position
    transform.position = Vector2.Lerp(transform.position, floatPosition, floatSpeed * Time.deltaTime);
    }


    void Shoot()
    {
        if (isReloading) return; // Prevent shooting if currently reloading

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
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
    isReloading = true;
    yield return new WaitForSeconds(cockTime);

    // Spawn the shotgun shell
    if (shellPrefab)
    {
        Vector2 ejectDirection = (firePoint.right + Vector3.down * 0.5f).normalized; // Adjust this for desired arc
        GameObject shell = Instantiate(shellPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();
        if (shellRb != null)
        {
            shellRb.AddForce(ejectDirection * 2f, ForceMode2D.Impulse); // Adjust force as needed
        }
    }

    // Play reload sound
    if (reloadSound)
    {
        AudioSource.PlayClipAtPoint(reloadSound, transform.position); // You can also use an AudioSource component on the GameObject
    }

    isReloading = false;
    }
}
