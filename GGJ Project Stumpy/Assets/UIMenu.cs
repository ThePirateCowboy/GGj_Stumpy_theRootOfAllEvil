using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelToLoadOnStart = "Test Movement";
    public Animator anim;
    public AudioClip[] clips1;
    public AudioClip[] clips2;
    public AudioSource audioSource;

    private AudioSource[] audioSources;
    void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }
    public void PlayClip1()
    {
        AudioClip clip1 = clips1[Random.Range(0, clips1.Length)];
        audioSource.clip = clip1;
        audioSource.Play();
    }
    public void PlayClip2()
    {
        AudioClip clip2 = clips2[Random.Range(0, clips2.Length)];
        audioSource.clip = clip2;
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene()
    {
        anim.SetTrigger("FadeOut");
        SceneManager.LoadScene(levelToLoadOnStart);
        
    }

    public void Quitting()
    {
        Application.Quit();
    }
    public void PauseAllAudioSources()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }
    }
    public void StopAllAudioSources()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

}
