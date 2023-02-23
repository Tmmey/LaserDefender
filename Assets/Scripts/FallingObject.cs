using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float fallSpeed = 3;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = new Vector2(0f, -fallSpeed);
    }

    void Update()
    {
        if (rb.position.y < -20)
        {
            Destroy(gameObject);
        }
    }
}
