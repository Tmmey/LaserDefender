using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] public bool isPlayer;
    [SerializeField] int health = 50;
    [SerializeField] int score = 50;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem shieldEffect;
    [SerializeField] float effectYOffset = 0f;
    [SerializeField] bool applyCameraShake;
    [SerializeField] float barrelRollDuration = 2f;
    [SerializeField] float barrelRollCooldown = 5f;

    bool isEvading = false;
    int maxHealth = 50;
    float shieldDuration = 0f;
    float barrelRollInternalCooldown;
    Vector3 offsettedPosition;

    CameraShake cameraShake;
    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    Player player;
    Liutenant liutenant;
    [HideInInspector] public Transform mainSpriteTransfrom;
    //bool hasShield = false;

    //List<bool> shields;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
        liutenant = GetComponent<Liutenant>();
        mainSpriteTransfrom = GetComponentsInChildren<Transform>().FirstOrDefault(x => x.tag == "MainSprite");
        maxHealth = health;
        barrelRollInternalCooldown = 0;
        //shields = new List<bool>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        Rotator meteor = other.GetComponent<Rotator>();
        //var otherIsProjectile = other.transform.root.CompareTag("Projectile");

        if (!isPlayer && meteor != null)
            return;

        if (!isEvading)
        {
            if (damageDealer != null)
            {
                if (shieldDuration <= Mathf.Epsilon)
                {
                    TakeDamage(damageDealer.GetDamage());
                    PlayHitEffect();
                    ShakeCamera();
                }
                else
                {
                    PlayShieldEffect();
                }

                damageDealer.Hit();
            }
        }
    }

    private void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }

    void TakeDamage(int takenDamage)
    {
        health -= takenDamage;
        audioPlayer.PlayDamageTakenClipClip();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isPlayer)
        {
            scoreKeeper.ModifyScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }

        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, offsettedPosition, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    void PlayShieldEffect()
    {
        if (shieldEffect != null)
        {
            ParticleSystem instance = Instantiate(shieldEffect, offsettedPosition, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void IncreaseHealth(int amount)
    {
        if (isPlayer)
        {
            health = health + amount > maxHealth ? maxHealth : health + amount;
        }
        else
        {
            health = health + amount;
        }
    }

    private void Update()
    {
        offsettedPosition = new Vector3(transform.position.x, transform.position.y + effectYOffset, transform.position.z);

        if (isPlayer)
        {
            if (shieldDuration > Mathf.Epsilon)
            {
                shieldDuration -= Time.deltaTime;
            }

            //Debug.Log(barrelRollInternalCooldown);
            if (!isEvading && barrelRollInternalCooldown > Mathf.Epsilon)
            {
                barrelRollInternalCooldown -= Time.deltaTime;
            }

            if (isEvading)
            {
                PlayBarrelRoll();
            }

            //direkt, egy check olcsóbb, mint egy állítás?
            if (shieldDuration <= Mathf.Epsilon)
            {
                player.shield.enabled = false;
            }
        }
        else if (liutenant != null)
        {
            if (shieldDuration > Mathf.Epsilon)
            {
                shieldDuration -= Time.deltaTime;
            }

            //direkt, egy check olcsóbb, mint egy állítás?
            if (shieldDuration <= Mathf.Epsilon)
            {
                liutenant.shield.enabled = false;
            }
        }
    }

    public void AddShieldDuration(float amount)
    {
        shieldDuration += amount;
        //Debug.Log(amount);
        if (isPlayer && player != null)
        {
            player.shield.enabled = true;
        }
        else if (liutenant != null)
        {
            liutenant.shield.enabled = true;
            // Debug.Log("shield enabled");
        }
    }

    public float GetShieldDuration()
    {
        return shieldDuration;
    }

    public void DoBarrelRoll()
    {
        if (barrelRollInternalCooldown <= Mathf.Epsilon)
        {
            isEvading = true;
            StartCoroutine(EndRoll());
        }
    }

    public IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(barrelRollDuration);
        isEvading = false;
        barrelRollInternalCooldown = barrelRollCooldown;
        //Debug.Log("Barrel roll ended");
    }

    public void PlayBarrelRoll()
    {
        if (mainSpriteTransfrom != null) 
        {
            if (mainSpriteTransfrom.rotation.y <=  360)
            {
                mainSpriteTransfrom.Rotate(new Vector3(0, 1, 0));
            }
            else
            {
                mainSpriteTransfrom.Rotate(new Vector3(0, 0, 0));
            }
        }
    }

    // IEnumerator GoLeft()
    // {
    //     // T$$anonymous$$s will wait 1 second like Invoke could do, remove t$$anonymous$$s if you don't need it
    //     yield return new WaitForSeconds(barrelRollDuration);


    //     float timePassed = 0;
    //     if (timePassed < 3)
    //     {
    //         // Code to go left here
    //         timePassed += Time.deltaTime;

    //         yield return null;
    //     }
    // }

    // public void AddShield(float duration)
    // {
    //     StartCoroutine(AddShieldCR(duration));
    // }

    // public IEnumerator AddShieldCR(float duration)
    // {
    //     Debug.Log("Shield added");
    //     player.shield.enabled = true;
    //     shields.Add(true);
    //     //player shield enable
    //     //shield flag enable
    //     //várni duration secet
    //     //player shield disable
    //     yield return new WaitForSeconds(duration);
    //     if (shields.Any())
    //         shields.RemoveAt(0);
    //     if (!shields.Any())
    //         player.shield.enabled = false;

    //     Debug.Log("Shield disabled");
    // }
}