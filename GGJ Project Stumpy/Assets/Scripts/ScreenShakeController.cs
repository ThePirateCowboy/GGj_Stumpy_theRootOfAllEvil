using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ScreenShakeController : MonoBehaviour
{
    /* This script is in responsible for the camera shake logic. 
     * It has a debug function that shakes the camera also in order to fine tune the shake length and power. 
     * 
     */
    public static ScreenShakeController instance;

    [Header("Rotation")]

    [Tooltip("All shake rotation will be multiplied by this number.")]
    public float rotationMultiplier;


    [Tooltip("Allows for the 'K' button to trigger a sample shake, based on the below length and power, for debugging and fine tuning perposes.")]
    public bool debug;
    [Tooltip("Length (seconds) of sample shake")]
    [SerializeField] private float lengthOfShakeDebug;
    [Tooltip("Power of sample shake")]
    [SerializeField] private float powerOfShakeDebug;
    //*****************************************
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;
    

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (debug)
        {
            if(Keyboard.current.kKey.wasPressedThisFrame) // at the frame the K button is pressed...
            {
                StartShake(lengthOfShakeDebug, powerOfShakeDebug); 
            }
        }
    }

    private void LateUpdate() // 
    {
        if (shakeTimeRemaining > 0) //while the shake time is still counting down. 
        {
            shakeTimeRemaining -= Time.deltaTime; //countdown shakeTimeRemaining
            float xAmount = Random.Range(-1, 1) * shakePower; //creating random amounts to vary the shake time in x axis
            float yAmount = Random.Range(-1, 1) * shakePower; //creating random amounts to vary the shake power in x axis
            Vector3 CameraPos = CameraController.instance.smoothPositionCache; // storing the value of the cameraControllers smoothPositionCache
            transform.position = new Vector3 (CameraPos.x + xAmount, CameraPos.y +yAmount, CameraPos.z); // move the gameobject with this script as a component(Camera), to the smoothed position but add on the randomized x and y axis value from above.
            shakePower = Mathf.MoveTowards(shakePower, 0, shakeFadeTime * Time.deltaTime); // Slowly move the shake power toward 0
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFadeTime * rotationMultiplier * Time.deltaTime); // Slowly move the shake rotation toward 0
        }
        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1, 1)); // Rotate gameobject with this script as a component(Camera) on the z axis to the shake rotation
    }

    /// <summary>
    /// Start shake protocal using length and power given. 
    /// </summary>
    /// <param name="length"></param>
    /// <param name="power"></param>
    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
        shakeRotation = power * rotationMultiplier;
    }
}
