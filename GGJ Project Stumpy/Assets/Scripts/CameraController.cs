using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    /* This script controls the follow cam/main camera. 
     */
    
    public static CameraController instance; //making the CameraController and Instance


    [Header("Main Camera")]

    [Tooltip("This value decides how fast the camera is following the Camera Target.")]
    [Range(0, 20)] public float speedSmooth = 5f; // needs to be public. Accessed in PlayerController. Can be used to increase the speed of the camera. Eg. in a fall the camera should be quicker so the player can aim where he wants to land.




    public BoundsFinder boundsFinder;
    //*****************************************
    [HideInInspector] public Vector3 smoothPositionCache;
    public CameraTarget CamTarget;  //This is used to store a reference of the camera when found. 
    public float halfHeight, halfWidth; // These are used to store the value of the camera to the width of the camera camera 
    public BoxCollider2D boundsBox; // Used to store a reference to the boundary area
    private Transform MainCameraTransform; // Used to store a reference to the main Camera
    

    /* Singleton pattern
    * As my understanding of it: Basically saying that this script should be accessible from anywhere
    * there can only be one of these scripts. If another one appears destroy it
    */
    void Awake()
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

    // Call this as once at the start
    void Start()
    {
        //SetCameraDimensions(); // set dimensions
    }

    void LateUpdate()
    {
        if (MainCameraTransform != null && boundsBox !=null) // If there is a camera target and a boundsBox assigned then ... (Null check)
        {
            
            MoveCameraToSmoothedPosition(); // move that camera
        }
        
             else //If there is no CameraTarget
            {
                if (CamTarget != null) //If there is a camTarget assigned then....
                {
                    MainCameraTransform = CamTarget.transform;
                }
                else //if there is no object asssigned to the camera target then...
                {
                    CamTarget = FindObjectOfType<CameraTarget>(); // Find a camTarget 
                    return;
                }

                if (boundsBox == null) // null check to say if there is a bounds box do this...
                {
                    if (boundsFinder.closestCameraBoundsBox.GetComponent<BoxCollider2D>() != null)// ANOTHER null check. If the stored reference in the boundsFinderScript that holds the bounds collider is active.
                    {
                        boundsBox = boundsFinder.closestCameraBoundsBox.GetComponent<BoxCollider2D>();// assign the boundsBox collider to be that reference 
                    }
                }
                else // if not do nothing 
                {
                    return;
                }
            }
        }
    

    /// <summary>
    /// Lerps the main camera from its current postion (mainCameraTransform) to toward it's desired postion, the speed of which is based on the speedSmooth variable. This postion is clamped based on the halfWidth, halfHeight variables.
    /// </summary>
    private void MoveCameraToSmoothedPosition()
    {
        //Storing a new vector3 which should be clamped between the camera boundary
        Vector3 cameraTargetPositionSlerp = new Vector3(
                        Mathf.Clamp(MainCameraTransform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth), // 
                        Mathf.Clamp(MainCameraTransform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
                        transform.position.z);

        Vector3 desiredPosition = cameraTargetPositionSlerp; // setting the 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speedSmooth * Time.deltaTime);
        transform.position = smoothedPosition;
        smoothPositionCache = smoothedPosition;
    }

    /// <summary>
    /// Gets the camera dimensions from the settings on the camera object.
    /// </summary>
    //** This was originally determined based on the main camera being orthographic. Now the camera is Perspective based but method still functions tyhough im not sure it's precise.
    private void SetCameraDimensions()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }
}
