using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroysecond : MonoBehaviour
{
    public float timeBeforeDestroy;
    public AudioClip[] clips;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = GetRandomClip();
        source.Play();
        StartCoroutine(SelfDestruct());
    }

    private AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
    IEnumerator SelfDestruct ()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
        yield break;
    }
}
