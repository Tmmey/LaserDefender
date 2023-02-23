using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Liutenant : MonoBehaviour
{
    //[SerializeField] SpriteRenderer spriteRenderer;
    //public LiutenantPropertySO LiutenantPropertySO { get; set; }
    [SerializeField] float speed = 1f;
    public int propertyCount = 1;

    LiutenantSpawner liutenantSpawner;
    SpriteRenderer spriteRenderer;
    Vector2 minBounds;
    Vector2 maxBounds;
    [HideInInspector] public SpriteRenderer shield;

    private void Awake()
    {
        liutenantSpawner = FindObjectOfType<LiutenantSpawner>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();// GetComponent<SpriteRenderer>();
        shield = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(x => x.tag == "Shield");
        minBounds = liutenantSpawner.minBounds;
        maxBounds = liutenantSpawner.maxBounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitBounds();
        //Debug.Log(propertyCount);
        SetRandomProperties();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(minBounds, maxBounds, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }

    void SetRandomProperties()
    {
        var properties = liutenantSpawner.GetLiutenantPropertySOs();

        for (int i = 0; i < propertyCount; i++)
        {
            var index = Random.Range(0, properties.Count);

            //if all the properties are added, then add more extra health
            if (index <= 0)
            {
                var healthScript = GetComponent<HealthScript>();
                healthScript.IncreaseHealth((int)properties[index].Value);
            }

            var rolledProperty = properties[index];
            //set the color
            if (spriteRenderer != null)
            {
                spriteRenderer.color = rolledProperty.Color;
            }

            //go through the extra properties
            switch (rolledProperty.LPType)
            {
                case LPType.ExtraHealth:
                    var healthScript = GetComponent<HealthScript>();
                    healthScript.IncreaseHealth((int)rolledProperty.Value);
                    break;

                case LPType.HasTripleShoot:
                    var shooter = GetComponent<Shooter>();
                    shooter.AddTripleShootDuration(float.MaxValue);
                    break;

                case LPType.HasBigShoot:
                    var shooters = GetComponents<Shooter>();

                    foreach (var sh in shooters)
                    {
                        if (sh.IsBigShoot())
                        {
                            sh.enabled = true;
                        }
                        else
                        {
                            sh.enabled = false;
                        }
                    }
                    break;

                case LPType.HasShield:
                    var hs = GetComponent<HealthScript>();
                    hs.AddShieldDuration(10f);
                    break;

                case LPType.ExtraSpeed:
                    speed += (int)rolledProperty.Value;
                    break;

                default:
                    break;
            }

            properties.RemoveAt(index);
        }
    }

    // void InitBounds()
    // {
    //     Camera mainCamera = Camera.main;
    //     minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
    //     minBounds.x = minBounds.x + 1;
    //     //minBounds = new Vector3(-4, 0, 0);
    //     maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    //     maxBounds.x = maxBounds.x - 1f;
    //     var maxBoundY = maxBounds.y - 1f;
    //     maxBounds.y = maxBoundY;
    //     minBounds.y = maxBoundY;
    //     //maxBounds = new Vector3(4, 0, 0);
    // }

}
