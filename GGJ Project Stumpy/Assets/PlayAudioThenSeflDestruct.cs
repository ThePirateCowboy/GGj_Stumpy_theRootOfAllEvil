using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioThenSeflDestruct : MonoBehaviour
{
    public AudioClip[] audioClips;

    public AudioSource audioSource;

    private void Start()
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}

