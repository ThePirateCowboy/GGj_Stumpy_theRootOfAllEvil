using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Animator anim;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public float bounceForce = 20f;
    public float randomPitchRange = 0.1f;
    public float randomVolumeRange = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, bounceForce);
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.pitch = 1 + Random.Range(-randomPitchRange, randomPitchRange);
            audioSource.volume = 1 + Random.Range(-randomVolumeRange, randomVolumeRange);
            audioSource.Play();
            anim.SetTrigger("Bounce");
        }
    }
}
