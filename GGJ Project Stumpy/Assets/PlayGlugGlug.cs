using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGlugGlug : MonoBehaviour
{
    public AudioClip[] clips;
    public float randomPitchVariation = 0.1f;

    public AudioSource source;

    void Start()
    {
    }

    public void PlayRandom()
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip randomClip = clips[randomIndex];

        source.pitch = 1f + Random.Range(-randomPitchVariation, randomPitchVariation);
        source.PlayOneShot(randomClip);
    }
}
