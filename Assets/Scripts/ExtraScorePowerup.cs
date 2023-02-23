using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraScorePowerup : MonoBehaviour
{
    [SerializeField] int scoreAmount = 100;
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
        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        HealthScript player = other.GetComponent<HealthScript>();

        if (scoreKeeper && player != null && player.isPlayer)
        {
            scoreKeeper.ModifyScore(scoreAmount);
            //Debug.Log("Score added");
            Destroy(gameObject);
        }
    }
}
