using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiation : MonoBehaviour
{
    public AudioSource lullabye, flute, _waves;
    
    public MusicMakerWaves music;
    public Collider2D coll;
    public GameObject CameraMain, CameraCut;
    private bool isCutScene;
    public GameObject Carrot, slide1, slide2, slide3;
    public Animator anim;
    public WaveSpawner waveSpawner;

    private void Start()
    {
        RespawnController.instance.StoneHenge = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Has LEft The Nest");
            coll.enabled = false;
            if(!PlayerController.instance.hasMetCarrot)
            {
                flute.Play();
                SwitchCameras();
                PlayerController.instance.canMove = false;
                anim.SetTrigger("CanAppear");
                PlayerController.instance.transform.position = new Vector3(-6.25f, -0.5350f, 0f);
                PlayerController.instance.FlipSpriteRight();

            }

        }
    }
   

    public void SwitchCameras()
    {
        if(!isCutScene)
        {
            CameraMain.SetActive(false);
            CameraCut.SetActive(true);
            isCutScene = true;
        }else
        {
            CameraMain.SetActive(true);
            CameraCut.SetActive(false);
        }
    }

    public void ReturnToGame()
    {
        SwitchCameras();
        PlayerController.instance.canMove = true;
        lullabye.Stop();
        music.PlayClips();
        _waves.Play();
        Debug.Log("BacxkToGameTriggered.");

        waveSpawner.canSpawn = true;

    }



}
