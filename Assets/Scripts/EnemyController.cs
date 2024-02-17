using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour

{
    // Start is called before the first frame update
    public PlayerController player;
    private Rigidbody2D enemyRB;

    public int detectionrange;

    void Start()
    {
        player = GetComponent<PlayerController>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRB.velocity = player.rb.velocity;
    }
}
