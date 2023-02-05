using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance; //making an instance

    [Tooltip("The parent object that should be enabled on pause button.")]
    public GameObject pauseScreen, pauseprompt;
    //*****************************************
    [HideInInspector]public bool isPaused; // is accessing in the player controller to restrict movement during pause screen.
    private int timesPaused;
    public float CountdownQuitter = 15;
    private float QuitCounters;
    public bool CounterCanStart;
    public TextMeshProUGUI TextCounter;
    private bool once;
    public GameObject pauseUnactivate;
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

    public void PauseUnpauseInput(InputAction.CallbackContext context)
    {
        if (context.performed) // when the pause action button has been fully pressed.
        {
            QuitCounters = CountdownQuitter;
            TextCounter.text = QuitCounters.ToString("F1");
            CounterCanStart = true;
            PauseUnpause();
            

        }else if (context.canceled)
        {
            PauseUnpause();
            CounterCanStart = false;
        }
    }

    /// <summary>
    /// depending whether the game is paused or not the pause/unpause.
    /// </summary>
    public void PauseUnpause()
    {
        if (isPaused) // if already paused
        {
            if(timesPaused>= 3)
            {
                pauseprompt.SetActive(false);
            }
            else
            {
                timesPaused++;
            }
            

            isPaused = false;
            pauseScreen.SetActive(false); // enable the pause object
            pauseUnactivate.SetActive(true);
            Time.timeScale = 1f; // Set the time to pass regularly
        }
        else // if not already paused
        {
            isPaused = true;
            pauseUnactivate.SetActive(false);
            pauseScreen.SetActive(true); // enable the pause object
            Time.timeScale = 0f; //Set the time to stop
        }
    }

    public void Update()
    {
        if (CounterCanStart)
        {
            if(QuitCounters >0)
            {
                QuitCounters -= Time.unscaledDeltaTime;

                TextCounter.text = QuitCounters.ToString("F1");
            }
            else
            {
                if (!once)
                {
                    TextCounter.text = "0";
                    Application.Quit();
                    once = true;
                }
            }
        }
    }
   
}
