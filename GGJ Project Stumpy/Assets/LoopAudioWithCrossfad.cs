using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoopAudioWithCrossfad : MonoBehaviour
{
    public AudioClip clip;
    public float crossfadeTime = 1f;

    public AudioSource source1;
    public AudioSource source2;

    void Start()
    {

        source1.clip = clip;
        source2.clip = clip;

        source1.loop = false;
        source2.loop = false;

        source1.Play();
    }

    void Update()
    {
        if (!source1.isPlaying)
        {
            source1.volume = 1f;
            source2.volume = 0f;
            source2.Play();
            StartCoroutine(Crossfade());
        }
    }

    private IEnumerator Crossfade()
    {
        float t = 0f;
        while (t < crossfadeTime)
        {
            t += Time.deltaTime;
            source1.volume = 1f - (t / crossfadeTime);
            source2.volume = t / crossfadeTime;
            yield return null;
        }

        AudioSource temp = source1;
        source1 = source2;
        source2 = temp;
    }
}
