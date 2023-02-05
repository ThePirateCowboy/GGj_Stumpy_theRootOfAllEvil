using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] clips;
    public float rate = 1f;
    public float pitchMin = 1f;
    public float pitchMax = 1.1f;
    public float volumeMin = 0.8f;
    public float volumeMax = 1f;
    public bool canPlayFootsteps;

    public AudioSource source;
    private float timeSinceLastStep;

    void Start()
    {
        timeSinceLastStep = 0f;
    }

    void Update()
    { 
        if(PlayerController.instance.canMove)
        {
            if (canPlayFootsteps)
            {


                timeSinceLastStep += Time.deltaTime;

                if (timeSinceLastStep >= rate)
                {
                    PlayRandomFootsteps();
                    timeSinceLastStep = 0f;
                }
            }


        }
    }

    public void PlayRandomFootsteps()
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip randomClip = clips[randomIndex];

        float randomPitch = Random.Range(pitchMin, pitchMax);
        float randomVolume = Random.Range(volumeMin, volumeMax);

        source.pitch = randomPitch;
        source.volume = randomVolume;

        source.PlayOneShot(randomClip);
    }
}
    

