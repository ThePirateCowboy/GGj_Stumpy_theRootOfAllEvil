using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttackManager : MonoBehaviour
{
    /*This script is to manage melee logic. It registers which way the player is facing when the melee action is triggered and
     * plays an animation based on this input. 
     * 
     * The Player Input component on the player object calls a method from this script based on the Melee Action. 
     * 
     * For reference these script was taken and adaptedf from the following Git Repo https://github.com/I-Am-Err00r/Melee-Attack/blob/main/EnemyHealth.cs
    */


    [Header("On Hit Physics.")]

    [Tooltip("The default force given to the player Rigidbody2D on successfull hit with objects.")]
    public float defaultForce = 300;
    [Tooltip("The force given to the player Rigidbody2D on successfull hit with objects underneath the player.")]
    public float upwardsForce = 600;
    [Tooltip("The time frame(seconds) that the force stated above is applied for.")]
    public float movementTime = .1f;
    [Tooltip("The length of time(seconds) after a swipe that the player must wait until they can swipe again.")]
    public float meleeTime = 0.2f;
    //*****************************************
    private bool meleeAttack = true; // bool used to trigger the attack animations.
    public Animator meleeAnimator; // animator that holds the melee animations.
    private PlayerController character; // a reference to the PlayerController Script used to determine whether the input is being held up or down.
    private bool oneTime = true; // used to stop the melee from being triggered twice by holding down the Melee Action.
    private bool holdingUp; // Bool to store whether the the player inputy is being held UP or not.
    private bool holdingDown; // Bool to store whether the the player inputy is being held DOWN or not.
    private float meleeCounter; //Counter used to countdown from the meleeTime after each hit in order to prevent button mashing on melee.


    private void Awake()
    {
        character = GetComponent<PlayerController>(); //store the reference for the PlayerController
       // meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>(); //store the reference for the MeleeWeapon Script.
    }

    private void Start()
    {
        if (meleeAnimator != null)
        {
            meleeAnimator.keepAnimatorControllerStateOnDisable = true; // this null check allows the animation to be continued even after disabled so that we dont freeze. 
        }
    }

    private void Update()
    {
       CheckIfHoldingVertical(); //checks
       if (meleeCounter > 0)
       {
                meleeCounter -= Time.deltaTime; // countsdown the melee Counter
       }
    }

    /// <summary>
    /// Checks if the PlayerControllers inputy is being held down or up.Uses bools to store value
    /// </summary>
    public void CheckIfHoldingVertical()
    {
        if (character.inputY != 0)
        {
            if (character.inputY > .9f)
            {
                holdingUp = true;
                holdingDown = false;
            }
            else if (character.inputY < -.9f)
            {
                holdingDown = true;
                holdingUp = false;
            }
            else
            {
                holdingDown = false;
                holdingUp = false;
            }
        }
        else
        {
            holdingDown = false;
            holdingUp = false;
        }
    }

    /// <summary>
    /// Performes the melee attck. This method should be clled by the player input component under the Melee Attack event under the player umbrella. 
    /// </summary>
    /// <param name="context"></param>
    public void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed && oneTime && meleeCounter <=0 && !PlayerController.instance.isRooting) // when the melee action is fully pressed and etc.
        {
            meleeAttack = true; 
            oneTime = false;
            CheckInputAndAttack();
        }
        if (context.canceled) // when the melee action button is depressed
        {
            meleeAttack = false;
            oneTime = true;
        }
    }

    /// <summary>
    /// Checks which direction the player is pressing at the time that the meleeAttack is triggered and perfomes an animation based on the different combinations of which. 
    /// </summary>
    private void CheckInputAndAttack()
    {
        meleeCounter = meleeTime; //sets the timer to the meleeTime public var.
        //play sound
        if (meleeAttack && (character.inputY > 0 || holdingUp)) //Checks to see if meleeAttack is true and pressing up
        {
            meleeAnimator.SetTrigger("IsMeleeUP"); //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
        }
        else if ((meleeAttack && (character.inputY < 0 || holdingDown)) && !character.isGrounded) //Checks to see if meleeAttack is true and pressing down while also not grounded
        {
            meleeAnimator.SetTrigger("IsMeleeDown"); //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
        }
        else if (((meleeAttack && character.inputY == 0) //Checks to see if meleeAttack is true and not pressing any direction
            || meleeAttack && ((character.inputY < 0 || holdingDown) && character.isGrounded))) //OR if melee attack is true and pressing down while grounded
        {
            meleeAnimator.SetTrigger("IsMelee"); //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
        }
    }
}
