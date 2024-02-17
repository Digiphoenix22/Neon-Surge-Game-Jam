using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ShellRemoval : MonoBehaviour
{
    public float removalTime = 5f; // Time after which the shell is removed

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, removalTime); // Destroys this shell instance after a set time
    }
}

