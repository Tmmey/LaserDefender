using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> powerups;
    [SerializeField][Range(0, 1)] float dropchance = 0.2f;

    private void OnDestroy()
    {
        var positionBeforeDestroy = transform.position;
        if (!this.gameObject.scene.isLoaded) return;
        var currentPowerupIndex = Random.Range(0, powerups.Count);
        var dropVar = Random.Range(0f, 1f);

        //Debug.Log(dropVar);
        if (dropVar < dropchance)
        {
            Instantiate(powerups[currentPowerupIndex], positionBeforeDestroy, Quaternion.identity);
        }
    }
}
