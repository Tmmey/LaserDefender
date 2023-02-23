using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    [SerializeField] float projectileOffset = 0f;
    [SerializeField] bool isBigShoot = false;


    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 1f;
    [SerializeField] float minimumFiringRate = 0.5f;

    [Header("Triple shoot powerup")]
    [SerializeField] float leftWingCannonx = -0.8f;
    [SerializeField] float rightWingCannonx = 0.8f;
    Vector3 leftWingPos;
    Vector3 rightWingPos;

    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;
    float tripleShootDuration = 0f;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }


    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        leftWingPos = new Vector3(transform.position.x + leftWingCannonx, transform.position.y + projectileOffset, 0);
        rightWingPos = new Vector3(transform.position.x + rightWingCannonx, transform.position.y + projectileOffset, 0);

        if(tripleShootDuration > 0)
        {
            tripleShootDuration -= Time.deltaTime;
        }

        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            var offsettedTransformPosition = new Vector3(transform.position.x, transform.position.y + projectileOffset + transform.position.z);
            GameObject instance = Instantiate(projectilePrefab, offsettedTransformPosition, Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);

            //if (!useAI && tripleShootDuration > 0)
            if (tripleShootDuration > 0)
            {
                TripleShoot();
            }

            if (useAI)
            {
                float AIFiringRate = Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);
                baseFiringRate = Mathf.Clamp(baseFiringRate, minimumFiringRate, float.MaxValue);
            }

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(baseFiringRate);
        };
    }

    private void TripleShoot()
    {
        GameObject instance2 = null;
        GameObject instance3 = null;
        Rigidbody2D rb2 = null;
        Rigidbody2D rb3 = null;
        instance2 = Instantiate(projectilePrefab, leftWingPos, Quaternion.identity);
        instance3 = Instantiate(projectilePrefab, rightWingPos, Quaternion.identity);
        rb2 = instance2.GetComponent<Rigidbody2D>();
        rb3 = instance3.GetComponent<Rigidbody2D>();

        if (rb2 != null && rb3 != null)
        {
            rb2.velocity = transform.up * projectileSpeed;
            rb3.velocity = transform.up * projectileSpeed;
        }

        Destroy(instance2, projectileLifetime);
        Destroy(instance3, projectileLifetime);
    }

    public void AddTripleShootDuration(float amount)
    {
        //Debug.Log("Tripleshoot started");
        tripleShootDuration = amount;
    }

    public float GetTripleShootDuration()
    {
        return tripleShootDuration;
    }

    public bool IsBigShoot()
    {
        return isBigShoot;
    }
}
