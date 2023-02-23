using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Paddings")]
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;

    Vector2 rawInput;
    Vector2 minBounds;
    Vector2 maxBounds;
    Shooter shooter;
    HealthScript healthScript;
    [HideInInspector] public SpriteRenderer shield;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        healthScript = GetComponent<HealthScript>();
        shield = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(x => x.tag == "Shield");
    }

    void Start()
    {
        InitBounds();
    }

    void Update()
    {
        Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }

    void OnBarrelRoll()
    {
        Debug.Log("BarrelRollPressed");
        healthScript.DoBarrelRoll();
    }

    public void AddTripleShootDuration(float amount)
    {
        shooter.AddTripleShootDuration(amount);
    }

    public float GetTripleShootDuration()
    {
        return shooter.GetTripleShootDuration();
    }

    // public void AddShieldDuration(float amount)
    // {
    //     healthScript.AddShieldDuration(amount);
    // }

    // public float GetShieldDuration()
    // {
    //     return healthScript.GetShieldDuration();
    // }
}
