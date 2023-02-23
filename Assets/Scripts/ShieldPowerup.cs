using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    [SerializeField] float duration = 10f;
    //[SerializeField] float fallSpeed = 3;
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
        HealthScript hs = other.GetComponent<HealthScript>();
        //Debug.Log("Shield collided");
        if (hs != null && hs.isPlayer)
        {
            //Debug.Log("hs found");
            //hs.AddShield(duration);
            hs.AddShieldDuration(duration);
            //Debug.Log("Trippleshoot collided");
            Destroy(gameObject);
        }
    }
}
