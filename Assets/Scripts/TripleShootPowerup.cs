using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShootPowerup : MonoBehaviour
{
    [SerializeField] float duration = 10f;
    // [SerializeField] float fallSpeed = 3;
    // Rigidbody2D rb;

    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // void Update()
    // {
    //     rb.velocity = new Vector2(0f, -fallSpeed);

    //     if (rb.position.y < -20)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            player.AddTripleShootDuration(duration);
            //Debug.Log("Trippleshoot collided");
            Destroy(gameObject);
        }
    }
}
