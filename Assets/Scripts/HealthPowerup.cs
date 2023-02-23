using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    [SerializeField] int healthAmount = 30;
    // [SerializeField] float fallSpeed = 3;

    // Rigidbody2D rb;

    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // private void Start()
    // {
    //     rb.velocity = new Vector2(0f, -fallSpeed);
    // }

    // void Update()
    // {
    //     if (rb.position.y < -20)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthScript playerHealth = other.GetComponent<HealthScript>();

        if (playerHealth && playerHealth.isPlayer)
        {
            playerHealth.IncreaseHealth(healthAmount);
            //Debug.Log("Health restored");
            Destroy(gameObject);
        }

    }
}
