using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMakerWaves : MonoBehaviour
{
    /*
    public AudioClip[] audioClips;
    public int currentClipIndex = 0;
    public bool playOnStart = false;
    public bool transitionToNextClip = false;

    public  AudioSource audioSource;
    private bool isClipPlaying = false;

    private void Start()
    {

        if (playOnStart)
        {
            PlayCurrentClip();
        }
    }

    public void PlayCurrentClip()
    {
        if (!isClipPlaying)
        {
            isClipPlaying = true;
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void StopCurrentClip()
    {
        audioSource.Stop();
        isClipPlaying = false;
    }

    private void Update()
    {
        if (transitionToNextClip)
        {
            transitionToNextClip = false;
            StopCurrentClip();
            currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
            PlayCurrentClip();
        }

        if (!audioSource.isPlaying && isClipPlaying)
        {
            isClipPlaying = false;
        }
    }
}
    */

    public AudioClip[] audioClips;
    public int currentClipIndex = 0;
    public float fadeDuration = 1.0f;
    public bool transitionToNextClip = false;

    public AudioSource audioSource;
    private bool isClipPlaying = false;

    private void Start()
    {
        
    }

    public void PlayClips()
    {
        PlayCurrentClip();
    }

    public void PlayCurrentClip()
    {
        if (!isClipPlaying)
        {
            isClipPlaying = true;
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.loop = false;
            audioSource.Play();
            StartCoroutine(FadeIn());
        }
    }

    public void StopCurrentClip()
    {
        StopCoroutine(FadeOut());
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float startVolume = 0.0f;
        float targetVolume = 1.0f;

        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / fadeDuration;

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float targetVolume = 0.0f;

        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / fadeDuration;

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        audioSource.volume = targetVolume;
        audioSource.Stop();
        isClipPlaying = false;
        currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
        PlayCurrentClip();
    }

    private void Update()
    {
        if (transitionToNextClip)
        {
            transitionToNextClip = false;
            StopCurrentClip();
        }
    }
}
