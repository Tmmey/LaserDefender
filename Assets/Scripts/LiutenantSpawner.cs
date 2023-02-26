using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiutenantSpawner : MonoBehaviour
{
    [SerializeField] List<LiutenantPropertySO> AllProperties;
    [SerializeField] Liutenant LiutenantBase;

    [SerializeField] float spawnTimeInterval = 60f;
    [SerializeField] float spawnTimeVariance = 15f;
    [SerializeField] float minimumSpawnTime = 40f;

    public Vector2 minBounds;
    public Vector2 maxBounds;

    int liutenantPropertyCounter = 1;

    static LiutenantSpawner instance;
    public LiutenantSpawner GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        ManageSingleton();
        InitBounds();
    }

    void Start()
    {
        Invoke("SpawnRoot", 5);
        //StartCoroutine(SpawnCR());
        //Spawn();
    }

    void Update()
    {

    }

    void SpawnRoot()
    {
        StartCoroutine(SpawnCR());
    }

    IEnumerator SpawnCR()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(GetRandomSpawnTime());
        }
    }

    void Spawn()
    {
        //LiutenantBase.LiutenantPropertySO = AllProperties[0];
        //Debug.Log(AllProperties[0]);
        //Instantiate(LiutenantBase, new Vector3(0, 3, 0), Quaternion.Inverse(transform.rotation));
        LiutenantBase.propertyCount = liutenantPropertyCounter;
        ++liutenantPropertyCounter;
        Instantiate(LiutenantBase, new Vector3(maxBounds.x - 20, maxBounds.y - 20, 0), Quaternion.identity);
        //Debug.Log("maxBounds.x - 20: " + (maxBounds.x - 20) + "maxBounds.y - 20: " + (maxBounds.y - 20));
    }

    public float GetRandomSpawnTime()
    {
        float spawTime = Random.Range(spawnTimeInterval - spawnTimeVariance, spawnTimeInterval + spawnTimeVariance);
        Debug.Log(spawTime);
        return Mathf.Clamp(spawTime, minimumSpawnTime, float.MaxValue);
    }

    // private void GenerateLiuteantProperties()
    // {
    //     AllProperties = new List<LiutenantPropertySO>();
    //     var vals = System.Enum.GetValues(typeof(LPType));
    // }

    private void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<LiutenantPropertySO> GetLiutenantPropertySOs()
    {
        return AllProperties;
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        minBounds.x = minBounds.x + 1;
        //minBounds = new Vector3(-4, 0, 0);
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        maxBounds.x = maxBounds.x - 1f;
        var maxBoundY = maxBounds.y - 1f;
        maxBounds.y = maxBoundY;
        minBounds.y = maxBoundY;
        //maxBounds = new Vector3(4, 0, 0);
    }
}
