using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField][Range(0f, 1f)] float shootingVolume = 1f;

    [Header("Damage Taken")]
    [SerializeField] AudioClip damageTakenClip;
    [SerializeField][Range(0f, 1f)] float damageTakenVolume = 1f;

    //singleton v2
    static AudioPlayer instance;
    //singleton Get Instance
    public AudioPlayer GetInstance()
    {
        return instance;
    }

    private void Awake() {
        ManageSingleton();
    }

    private void ManageSingleton()
    {
        //singleton v1
        // int instanceCount = FindObjectsOfType(GetType()).Length;
        // if(instanceCount > 1)
        //singleton v2
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

    public void PlayShootingClip()
    {
        PlayClip(shootingClip, shootingVolume);
    }

    public void PlayDamageTakenClipClip()
    {
        PlayClip(damageTakenClip, damageTakenVolume);
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
