using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToAndFromBlack : MonoBehaviour
{
    //https://www.youtube.com/watch?v=Hn804Wgr3KE Useful for tips on UI Implementation with the new input system

    [Tooltip("The parent game object that holds the fadescreen image. There can only be one image under this object.")]
    public GameObject fadescreenObject;
    [Tooltip("Speed at which the fade to and from black occurs.")]
    public float fadeSpeed = 2f;


    

    public bool canPlayAudio;

    //The following are bools to show if we have completed the fading process
    private bool fadingToBlack, fadingFromBlack;
    // the amount of time(seconds) that has to have elapsed in order to call the OnButtonClick Wwise Event. 
    private float countdownForWwise = 1.5f;
    //a reference for the actualy image that fades to and from black.
    private Image fadeScreen;


    //On Awake
    private void Awake()
    {
        //store these reference
        fadeScreen = fadescreenObject.GetComponentInChildren<Image>();

        //Set FadeScreen Object to true. This allows us to work in the editor without being obstructed by the black screen. 
        fadescreenObject.SetActive(true);
    }
    // Start is called before the first frame update


    //Coroutine is called after awake method
    IEnumerator Start()
    {
        //Wait for a little bit...
        yield return new WaitForSeconds(0.7f);
        //call this method
        StartFadeFromBlack();
        
    }

    void Update()
    {
        //This is a way to ensure the first wwise event for the selction of the button doesnt just play while the screen is black
        if(countdownForWwise >0)
        {
            countdownForWwise -= Time.deltaTime;
        }

        //If the fadingToBlack bool is true...
        if (fadingToBlack)
        {
            // change the Alpha color of the image fadescreen toward a non transparent color. 
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            //Once the color of the fadescreen has reached the desired non transparent color...
            if (fadeScreen.color.a == 1f)
            {
                //Stop fading to black by deactivating the bool
                fadingToBlack = false;
            }
        }
        else if (fadingFromBlack)
        {
            // change the Alpha color of the image fadescreen toward a transparent color. 
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            //Once the color of the fadescreen has reached the desired transparent color...
            if (fadeScreen.color.a == 0f)
            {
                //Stop fading from black by deactivating the bool
                fadingFromBlack = false;

                //Disable the Fadescreen Object. This si done in order to not obstruct any other UI elements. 
                fadescreenObject.SetActive(false);
            }
        }
    }

    //This method begins the process of fading to black
    public void StartFadeToBlack()
    {
        //Set the fadescrren parent object to active
        fadescreenObject.SetActive(true);

        

        //Enable the fade to black bool
        fadingToBlack = true;

        //Disable the fade From black bool
        fadingFromBlack = false;
    }

    //This method begins the process of fading from black
    public void StartFadeFromBlack()
    {
        //Disable the fade to black bool
        fadingToBlack = false;
        
        //Enable the fade from black bool
        fadingFromBlack = true;
        
    }
}
