using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundsFinder : MonoBehaviour
{

    /* The BoundsFinder Script is used to find the camera bounds that is closest to the player object. 
     * It sets this found object, and more importantly its collider component to be the new boundary. 
     * This boundary is used by the CameraController Script to clamp the camera movement so it doesnt 
     * go outside of the intended boundary.
     */

	public static BoundsFinder instance; //makes this script an instance.


	[Tooltip("Enable to allow a line to be drawn in the inspector window between the player and the closest object with a cameraBoundaryTarget script on it.")]
	[SerializeField] private bool debug;
    //*****************************************
    [HideInInspector] public CameraBoundaryTarget closestCameraBoundsBox; //reference for the closest CameraBoundsBox script
    private GameObject thePlayer; //target from which closest is determined.

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
	
	private void OnEnable()
    {
		SceneManager.sceneLoaded += this.OnLevelCallback; //Register the OnLevelCallback method to be called...once the scene is completely loaded.
    }
	
	private void OnDisable()
    {
		SceneManager.sceneLoaded -= this.OnLevelCallback; //Register the OnLevelCallback method to be called...once the scene is completely loaded. ITs good practise apparently to unregister from events that you register to. 
    }

    /// <summary>
    /// We registered in the OnEnable method to call this event when the scene loading is completed as mentioned above. 
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="sceneMode"></param>
    private void OnLevelCallback(Scene scene, LoadSceneMode sceneMode)
    {
		
		thePlayer = PlayerController.instance.gameObject; // Registers thePlayer to be the gameobject that has the instanced script PlayerController on it. 
        FindClosestBounds(); 
    }

	/// <summary>
	/// Finds the closest camera boundary, via the CameraBoundaryTarget script, and stores it in public reference, which is accessed in the camera controller script.
	/// </summary>
	void FindClosestBounds()
    {
        float distanceToClosestBoundaryBoxScript = Mathf.Infinity;
        CameraBoundaryTarget cameraBoundaryTarget = null;
        CameraBoundaryTarget[] allEnemies = GameObject.FindObjectsOfType<CameraBoundaryTarget>(); //Store all objects in scene that have dialog script components in array. 

        //For every dialog cript in array and therefore heirarchy
        foreach (CameraBoundaryTarget currentDialogue in allEnemies)
        {
            //create a refernce for the distance to a given object in array with dialog script
            float distanceToDialog = (currentDialogue.transform.position - thePlayer.transform.position).sqrMagnitude;

            // if the distance is less than the distance to the closest
            if (distanceToDialog < distanceToClosestBoundaryBoxScript)
            {
                distanceToClosestBoundaryBoxScript = distanceToDialog; // This distance is the new closest
                cameraBoundaryTarget = currentDialogue; // Set the closest to be the current active 
                closestCameraBoundsBox = cameraBoundaryTarget; // Storing closest Dialogue Object in reference "closestDialogue".
            }
        }
        DrawLine(cameraBoundaryTarget); 
    }

    /// <summary>
    /// When debug bool is enabled draws a line in Unity editor between thePlayer object and the closest Dialog script.
    /// </summary>
    private void DrawLine(CameraBoundaryTarget cameraBoundaryTarget)
    {
        
        if (debug)
        {
            Debug.DrawLine(thePlayer.transform.position, cameraBoundaryTarget.transform.position);
        }
    }
}
