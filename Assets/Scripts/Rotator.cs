using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float rotateValue;
    // Start is called before the first frame update
    void Start()
    {
        rotateValue = Random.Range(-1f, 1f);   
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateValue, Space.World);
    }
}
