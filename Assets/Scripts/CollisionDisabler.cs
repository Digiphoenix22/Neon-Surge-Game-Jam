using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDisabler : MonoBehaviour
{
    public GameObject boss; // Assign the boss GameObject in the inspector
    public Collider2D targetCollider; // Assign the target Collider2D you want to disable in the inspector

    void Update()
    {
        // Check if the boss has been destroyed
        if (boss == null && targetCollider != null)
        {
            targetCollider.enabled = false; // Disable the target collider
            // Optionally, destroy this script or GameObject if no longer needed
            Destroy(this); // Removes this script component
            // OR
            //Destroy(gameObject); // Removes the GameObject this script is attached to
        }
    }
}
