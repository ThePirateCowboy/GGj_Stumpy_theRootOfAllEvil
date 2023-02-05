using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StonesAppear : MonoBehaviour
{

    public GameObject StoneA, StoneB, StoneC, StoneD;

    public int threshhold = 5;
    public int current;
    public int stage;

    public AudioClip[] clips;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current>= threshhold)
        {
            if (stage == 0)
            {
                stage++;
                current = 0;
                StoneA.SetActive(true);
                PlayRandomClip();
            }
            else if (stage == 1)
            {
                stage++;
                current = 0;
                StoneB.SetActive(true);
                PlayRandomClip();
            }
            else 
            if (stage == 2)
            {
                stage++;
                current = 0;
                StoneC.SetActive(true);
                PlayRandomClip();
            }
            else 
            if (stage == 3)
            {
                StartCoroutine(EndOfTimes());
                PauseMenu.instance.StopAllAudioSources();
                StoneD.SetActive(true);
                PlayRandomClip();
            }
        }
        if(RespawnController.instance.StoneHenge)
        {
            SetStoneHenge();
        }
    }
    
    public void SetStoneHenge()
    {
        StoneA.SetActive(true);
        StoneC.SetActive(true);
        StoneD.SetActive(true);
        StoneB.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            current += other.GetComponent<EnemyHealth>().enemyValue;
            Destroy(other.gameObject);
        }
    }
    public void PlayRandomClip()
    {
        source.clip = GetRandomClip();
        source.Play();
    }

    private AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }

    IEnumerator EndOfTimes()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Ending Bad");
    }
}
