using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    /*This script is in charge of the collision of the melee weapon.
     * How much damage it does 
     * 
     * 
     * References:
     * 
     * https://github.com/I-Am-Err00r/Melee-Attack/blob/main/EnemyHealth.cs
     */
    [Header("Melee")]
    [Tooltip("Amount of damage that is done by the melee attack.")]
    [SerializeField] private int damageAmount = 20;
    //*****************************************
    private PlayerController playerController; //store of the playerController
    private Rigidbody2D rb; // store of the Rigidbody2D on the Player object. 
    private MeleeAttackManager meleeAttackManager; //stores the meleeAttackManager
    private Vector2 direction; //stores the direction on x and y axis
    private bool collided; //registers if the melee weapon has collided in order to not make multiple collisions on the same strike.
    private bool downwardStrike; // registers if the strike was meant to be a downward strike.
    private bool hasStarted; // registers if the strike was meant to be a upward strike. 


    private void OnEnable()
    {
        if (hasStarted)
        {
            rb.velocity = Vector2.zero; // resets the velocity of the Rigidbody2D on enable. 
            StartCoroutine(NoLongerColliding()); // 
        }
    }
    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();// store playerController from parent object
        rb = GetComponentInParent<Rigidbody2D>(); //stores RigidBody2D from parent
        meleeAttackManager = GetComponentInParent<MeleeAttackManager>(); //stores the meleeAttackManager
        hasStarted = true; // This bool is used to reset 
    }

    private void FixedUpdate()
    {
        HandleMovement();
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyHealth>()) //if the object that collides with the trigger has a EnemyHealth script on it.
        {
            HandleCollision(other.GetComponent<EnemyHealth>());
        }
    }
    /// <summary>
    /// Handles the force that is given to player Rigidbody2D based on what they hit and which direction they hit it.
    /// </summary>
    /// <param name="objHealth"></param>
    private void HandleCollision(EnemyHealth objHealth)
    {
        if (objHealth.giveUpwardForce && playerController.inputY < 0 && !playerController.isGrounded) //if the object that is colliding with has giveUpwardForce bool enabled, the player is pressing down and the player is not grounded
        {
            direction = Vector2.up; //set direction to be upward
            downwardStrike = true; 
            collided = true;
        }
        if (playerController.inputY > 0 && !playerController.isGrounded) //if the player is holding up, and is not grounded
        {
            direction = Vector2.down; //direction down
            collided = true; //
        }
        if ((playerController.inputY <= 0 && playerController.isGrounded) || playerController.inputY == 0) // if player is holding down and/or neither up or down and the player is grounded
        {
            if (playerController.facingLeft) //if the player is facing left
            {
                direction = Vector2.right; //direction should be right
            }
            else
            {
                direction = Vector2.left; //The direction should be left
            }
            collided = true; 
        }
        objHealth.Damage(damageAmount); //Damage the object the damageAmount.
        StartCoroutine(NoLongerColliding()); 
    }
    /// <summary>
    /// Gives force in the given direction stored in the HandleCollision method.
    /// </summary>
    private void HandleMovement()
    {
        if (collided)
        {
            if (downwardStrike)
            {
                rb.velocity = new Vector2(rb.velocity.x, meleeAttackManager.upwardsForce); // upforce is stronger than the default force so this is the logic for the upward force
            }
            else
            {
                rb.AddForce(direction * meleeAttackManager.defaultForce);// and this is the logic for the default force. 
            }
        }
    }
    /// <summary>
    /// Waits for the movement time and then stops 
    /// </summary>
    /// <returns></returns>
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSeconds(meleeAttackManager.movementTime);
        collided = false;
        downwardStrike = false;
    }
}
