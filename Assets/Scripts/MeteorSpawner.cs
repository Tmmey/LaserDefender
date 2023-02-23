using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] float timeBetweenSpawns = 10f;
    [SerializeField] float spawnTimeVariance = 3f;
    [SerializeField] float minimumSpawnTime = 3f;
    [SerializeField] List<GameObject> meteors;

    float cameraLeftCorner;
    float cameraRightCorner;
    float cameraTop;
    
    void Start()
    {
        
        var cameraParams = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        cameraLeftCorner = -cameraParams.x;
        cameraRightCorner = cameraParams.x;
        cameraTop = cameraParams.y;
        StartCoroutine(SpawnMeteorsCR());
    }

    void Update()
    {

    }

    IEnumerator SpawnMeteorsCR()
    {
        while (true)
        {
            var pos = Random.Range(cameraLeftCorner + 0.5f, cameraRightCorner - 0.5f);
            var index = Random.Range(0, meteors.Count);
            //Debug.Log(index);
            
            if (meteors.Any())
            {
                Instantiate(meteors[index], new Vector3(pos, cameraTop, 0), Quaternion.identity);
            }

            yield return new WaitForSeconds(GetRandomSpawnTime());
        }
    }

    public float GetRandomSpawnTime()
    {
        float spawTime = Random.Range(timeBetweenSpawns - spawnTimeVariance, timeBetweenSpawns + spawnTimeVariance);
        return Mathf.Clamp(spawTime, minimumSpawnTime, float.MaxValue);
    }
}
