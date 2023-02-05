using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip audioClip;
    public float fadeDuration = 1.0f;

    public AudioSource audioSource;
    private bool isClipPlaying = false;

    private void Start()
    {
    }

    public void StartAudioClip()
    {
        if (!isClipPlaying)
        {
            isClipPlaying = true;
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
            StartCoroutine(FadeIn());
        }
    }

    public void StopAudioClip()
    {
        if (isClipPlaying)
        {
            isClipPlaying = false;
            StartCoroutine(FadeOut());
        }
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
    }
}
