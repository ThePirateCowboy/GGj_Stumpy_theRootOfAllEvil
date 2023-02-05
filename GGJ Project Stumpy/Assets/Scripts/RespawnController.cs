using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance; // making an instance.
    public AudioClip[] clips;
    public AudioSource source;
    private AudioSource[] sources;
    public float fadeOutTime = 1f;
    //*****************************************
    [HideInInspector] public bool shouldfaceLeft; // This bool is used in to store whether the player should be facing left or facing right on respawn.
    private Vector3 respawnPoint; // The point at whihc the repawn will spawn the player.  
    private GameObject thePlayer; // Store for the player Game Object.
    public bool StoneHenge = false;
    /* Singleton pattern
    * As my understanding of it: Basically saying that this script should be accessible from anywhere
    * there can only be one of these scripts. If another one appears destroy it
    */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        sources = FindObjectsOfType<AudioSource>();
        thePlayer = PlayerHealthController.instance.gameObject; //stores the player object
        respawnPoint = thePlayer.transform.position; //if first time loading set spawn point to the original position.
    }

    /// <summary>
    /// Sets the spawn point to be the newPosition, and states whteher they should be facing left or right on spawn.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="faceLeft"></param>
    public void SetSpawn(Vector3 newPosition, bool faceLeft)
    {
        respawnPoint = newPosition;
        shouldfaceLeft = faceLeft;
    }

    /// <summary>
    /// Begins the respawn logic.
    /// </summary>
    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    /// <summary>
    /// repawns the player by fading the screen to black, deactivating the player object loading the scene again, moving the player to the spawn point and then re-enabling the player object.
    /// </summary>
    /// <returns></returns>
    IEnumerator RespawnCo()
    {
        PlayerController.instance.isRespawning = true; //register the player as in the middle of the respawn process.
        PlayRandomClip();
        PlayerController.instance.canMove = false; //stop the player from moving
        yield return new WaitForSeconds(2);
        FadeOutAll();
        UIController.instance.StartFadeToBlack(); //fade to black
        thePlayer.SetActive(false); //disable the playerController
        yield return new WaitForSeconds(1f - ((UIController.instance.fadeSpeed/3))); //wait for given time
        
        thePlayer.transform.position = respawnPoint; // postion te player at the respawn point
        PlayerController.instance.RootBeerAmount = 0;
        PlayerController.instance.UpdateRootbeerAmountAndUI();
        yield return new WaitForSeconds((1.5f / UIController.instance.fadeSpeed) -1f); // wait for the given time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // load the current scene again. 
        UIController.instance.StartFadeFromBlack(); // fade from black
        PlayerController.instance.playerAnimator.SetTrigger("IsRespawned");
        thePlayer.SetActive(true); //Enable the player object
        PlayerController.instance.ResetPlayer(); //resets the players state to default. 
        PlayerHealthController.instance.FillHealth(); // fills health to max
        PlayerController.instance.canMove = true; // enables movement again. 
    }

    /// <summary>
    /// Loads the scene, levelToLoad, asynchronously 
    /// </summary>
    /// <param name="levelToLoad"></param>
    public void LoadSceneAsyncMethod(string levelToLoad)
    {
        StartCoroutine(LoadSceneAsync(levelToLoad));

    }

    /// <summary>
    /// Corotuine Loads Scene And waits for it to be done. When it is done fade up from black and allow the player to move. 
    /// </summary>
    /// <param name="levelToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(string levelToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(.8f);
        UIController.instance.StartFadeFromBlack();
        PlayerController.instance.canMove = true;
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
    public void FadeOutAll()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            foreach (AudioSource source in sources)
            {
                source.volume = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            }
            yield return null;
        }
    }

}

