using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Image fadeScreen;
    public GameObject fadescreenObject;
    public float fadeSpeed = 2f;
    private bool fadingToBlack, fadingFromBlack;


    [SerializeField] private bool fadeFromStart;
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
        fadescreenObject.SetActive(true);
    }

    private void Start()
    {

        if (fadeFromStart)
        {
            StartFadeFromBlack();
        }
    }
    void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed+.4f * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }
        }
        if (fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }
    }
    
    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }
    public void StartFadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
    }
    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
