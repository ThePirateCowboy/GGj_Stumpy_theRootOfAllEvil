using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioVines : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public void PlayRandomAudio()
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
}
