using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{


    /* The PlayerController script deals with the player movement (Horizontal Movement, Jump, Double Jump, Dash, Fall Speed)
     * It deals with the particle efffects and audio that are triggered by the movement and on the player object. 
     * It holds references to the Dialogue Manager and enables the interaction prompt based on a bool inDialogueArea.
     * 
     * The Player Input component on the player object calls a method from this script based on the Movement, Jump & Dash. 
     * 
     * References:
     * "How To Use The New Unity Input System In A Platformer" - https://www.youtube.com/watch?v=1QWjm6yVp3g
     * Really deep dive on the input system - https://www.youtube.com/watch?v=o9wauUU7nmg
    */

    //Rooting
    public AudioClip[] jump;
    public AudioSource JumpSource;
    public AudioClip earthQuakeClip;
    public AudioSource EarthQuakeClipSource;
    public AudioClip[] RootEarthQuake;
    public AudioSource source;
    public Footsteps footsteps;
    public bool hasMetCarrot;
    [Header("Rooting Abilities")]
    public bool isRooting;
    public float cooldownToRootAbility = 3;
    private float cooldownToRootAbilityCounter;
    public Animator rightWall, leftWall, leftWallFar, rightWallFar;
    public float vulnerableTime = 4;
    public  float vulneableTimeCountdown;
    public int ZoneNumber;
    public int RootBeerAmount = 0;
    public Animator RootbeerUI;
   



    public static PlayerController instance; // Making the script and instance 
    public float cameraShakeLength, cameraShakePower;

    [Header("Abilities")]

    [Tooltip("Enables double jump ability")]
    public bool doubleJumpAbility = true;
    
    [Header("Effects")]
    [Tooltip("Enables the dust effect on Movement and grounded")]
    public bool canMakeDust = true;
   

    [Header("Player Physics")]
    public Animator playerAnimator;
    [Tooltip("The RigidBody")]
    public Rigidbody2D theRB; //Gets stored in the Start method.
    [Tooltip("The speed at which the player can move on the x axis.")]
    [SerializeField] private float moveSpeed;
    [Tooltip("The point in space that the groundedCheckRadius is drawn around in the CheckCollisions method, that determines if the player is grounded or not. IMPORTANT: Should be a child of the player.")]
    [SerializeField] private Transform groundPoint;
    [Tooltip("The length of the radius used draw the circle, in CheckCollisions method around the groundPoint.")]
    [SerializeField] private float groundCheckRadius = .1f;
    [Tooltip("The layers that should be considered to be ground. Used in the CheckCollisions method to determine in the isGrounded status.")]
    [SerializeField] private LayerMask whatisGround;
    [Tooltip("The maximuim speed that the player can fall at.")]
    [SerializeField] private float maxFallSpeed = 1;
    [Tooltip("During a fall, the default camera follow speed is multiplied by this number and lerped toward.")]
    [SerializeField] [Range(1,5)] private float cameraFallSpeedMultiplier;
    [Tooltip("This number is used to smooth the speed of the transition between the default camera follow speed and the speed when falling.")]
    [SerializeField] private float fallSpeedSmooth;
    [Tooltip("Physics material that should be applied when not grounded")]
    [SerializeField] private PhysicsMaterial2D slippy;
    [Tooltip("Physics material that should be applied when ground")]
    [SerializeField] private PhysicsMaterial2D notSoSlippy;
    [Tooltip("This particle system simulates dust/leaves being propelled into the air behind the playeras the player travels along the ground.")]
    [SerializeField] private ParticleSystem dustEffect;
    [Tooltip("This number determines the rate at which the dustEffect can emit particles")]
    [SerializeField] private float footstepRateOverdistance;
    //*****************************************
    public bool isGrounded; // Stores the status of the player being on the ground.
    [HideInInspector] public Vector3 localScaleAtStart; // Stores the local scale values of the player in the start function. Used in the FlipSpriteLeft & FlipSpriteRight methods.
    public bool facingLeft; //Stores whether the player is facing left or right.
    private Collider2D coll; // Stored reference to the collider object on the player used to switch the physics materials when grounded status changes 
    private bool wasOnGround; //Stores the status of whether the player was on the ground in the last frame.
    private ParticleSystem.EmissionModule footEmmision; // stores the emmision module of the dustEffect particle system. May not be necessary.
    private float gravityAtStart; // stores the gravity value on the RigidBody2D of the player at the start function in order to reset it to default.
    private float smoothSpeedCache; // Stores the value of the UIController.smoothSpeed in order to reset it to default upon changing.
    private bool isRegisteredAsNotFalling; // Is used to register if the player is falling.


    [Header("JumpControl")]

    [Tooltip("The amount of force that is applied upward on a jump.")]
    [SerializeField] private float jumpForce;
    [Tooltip("The window of time (seconds) during which you can still execute a jump after running off a ledge.")]
    [SerializeField] private float coyoteTime = .18f;
    [Tooltip("This window of time (seconds) before the player hits the ground that the jump action will be recorded and set to trigger when the player does hit the ground.")]
    [SerializeField] private float jumpBufferTime = .1f;
    [Tooltip("This is the percentage of the default anitgravitional pull/downforce, being applied to the player, that is on applied on release of the jump button. 100% being no decrease to default original gravitational pull.")]
    [SerializeField] [Range(0, 100)] private float DownforcePostRelease = 35f;
    [Tooltip("The effect that is triggered when the Player initially lands on the ground.")]
    [SerializeField] private ParticleSystem impactEffect;
    //*****************************************
    private float coyoteTimeCounter;// Is used to countdown the coyoteTime once being ungrounded. 
    [SerializeField] private float jumpBufferCounter;// Is used to countdown the jumpBufferTime once being ungrounded.
    private bool isJumping; // bool that registers when the player is jumping.





    [Header("KnockBack/Bounce")]

    [Tooltip("The length of time (seconds) that the player is knocked back for when they recieve a knock back")]
    [SerializeField] private float knockBackLength = .25f;
    [Tooltip("The force that is applied to the player when they recieve a knock back")]
    [SerializeField] private float knockBackForce = 5;
    [Tooltip("Upward force applied when bouncing on Enemy or Spring")]
    [SerializeField] private float bounceForce;
    //*****************************************
    private float knockBackCounter; // counter used to countdown window of time knockback is active. 


    [Header("Input")]

    [Tooltip("Holds the x axis input from the Movement input device")]
     public float inputX; //Stored input value for the x axis
    [Tooltip("Holds the y axis input from the Movement input device")]
     public float inputY; //Stored input value for the y axis
    
    
    [Header("Dialogue")]

    [Tooltip("Should be as gameobject on the player that holds canvas or UI element that is indicates there is an interaction possible.")]
    public GameObject interactionIndicator;
    


    [Header("Player State")]

    [Tooltip("Is the player currently respawning.")]
    [HideInInspector] public bool isRespawning;
    [Tooltip("Stops the ability move when disabled")]
    [HideInInspector] public bool canMove = true;
    private bool hasStarted;

    [Header("Audio")]

    [HideInInspector] public bool gameHasLoadedIntoFirstLevel; // records if the game has loaded. This is used in the 

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
    // Start is called before the first frame update
    void Start()
    {
        StoreVariables();
        coll.sharedMaterial = notSoSlippy;
        hasStarted = true;
    }

    private void OnEnable() 
    {
        if(hasStarted)
        {
            ResetPlayer();
        }
        if (isRespawning)
        {
            transform.localScale = localScaleAtStart;
            if (RespawnController.instance.shouldfaceLeft)
            {
                FlipSpriteLeft();
            }
            else
            {
                FlipSpriteRight();
            }
            isRespawning = false;
        }
    }

    private void OnDisable()
    {
        transform.localScale = localScaleAtStart;
    }


    public void PlayRootBeerClip()
    {
        source.clip = GetRandomClip();
        source.Play();
    }

    private AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, RootEarthQuake.Length);
        return RootEarthQuake[randomIndex];
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!canMove)
        {
            playerAnimator.SetBool("IsMoving", false);
            theRB.velocity = new Vector2(0, 0);
        }
       if (vulneableTimeCountdown >0)
        {
            vulneableTimeCountdown -= Time.deltaTime;
        } else
        {
            isRooting = false;
        }
        if (!PauseMenu.instance.isPaused && canMove && vulneableTimeCountdown <= 0)
        {
            ClampFallSpeed();
            IsOnWayDown();

            if (knockBackCounter <= 0)
            {
                if (cooldownToRootAbilityCounter >0)
                {
                    cooldownToRootAbilityCounter -= Time.deltaTime;
                }
                
               
                HorizontalMovement();
                CheckCollisions();
                WhenGroundedThisFrame();
                WhenNotGrounded();
                CountdownJumpBuffer();
                PlayerFacingLeftOrRight();
                if(isJumping)
                {
                    if (jumpBufferCounter >= 0f && coyoteTimeCounter > 0f)
                    {
                        
                        PlayJumpClip();
                        playerAnimator.SetBool("IsJumping", true);
                        playerAnimator.SetBool("IsGrounded", false);
                        playerAnimator.SetBool("IsFalling", false);

                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        jumpBufferCounter = 0f;
                        isJumping = false;
                    }
                }
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
            }
            PlayParticlesOnMoveOnGround();
            wasOnGround = isGrounded; //Registers if the player was grounded this frame. 
        }
    }

    public void PlayEarthQuakeClip()
    {
                EarthQuakeClipSource.clip = earthQuakeClip;
                EarthQuakeClipSource.Play();
    }
    /// <summary>
    /// This method stores info in our variables for later use 
    /// </summary>
    /// 
    private void StoreVariables()
    {
        coll = GetComponent<Collider2D>();
        gravityAtStart = theRB.gravityScale;
        footEmmision = dustEffect.emission;
        localScaleAtStart = gameObject.transform.localScale;
        var mainCam = FindObjectOfType<CameraController>();
        smoothSpeedCache = mainCam.speedSmooth;
    }
    /// <summary>
    /// Deals with turning the player left or right. 
    /// </summary>
    private void PlayerFacingLeftOrRight()
    {
        //Appear to turn the sprite to reflect the direction the player is facing
        if (theRB.velocity.x > 0f && facingLeft)
        {
            FlipSpriteRight();
        }
        else if (theRB.velocity.x < 0f && !facingLeft)
        {
            FlipSpriteLeft();
           
        }
    }
   

    /// <summary>
    /// Logic that is executed when the player is grounded but wasnt grounded the frame before.
    /// </summary>
    private void WhenGroundedThisFrame()
    {
        //When grounded this frame but wasnt last.
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (!wasOnGround)
            {
                ////////////////////////////////////////////////////////////////////////////
                playerAnimator.SetBool("IsGrounded", true);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsFalling", false);
                coll.sharedMaterial = notSoSlippy;
                if (canMakeDust)
                {
                    impactEffect.gameObject.SetActive(true);
                    impactEffect.Stop();
                    impactEffect.transform.position = dustEffect.transform.position;
                    impactEffect.Play();
                }
            }
        }
    }
    /// <summary>
    /// Logic that is executed when the player is not grounded. 
    /// </summary>
    private void WhenNotGrounded()
    {
        if (!isGrounded)
        {
            
            
            footsteps.canPlayFootsteps = false;
            
            if (wasOnGround)
            {
                coll.sharedMaterial = slippy;
            }
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    /// <summary>
    /// Countdown the jump buffer timer 
    /// </summary>
    private void CountdownJumpBuffer()
    {
        //counts down the jump buffer counter
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        else
        {
            jumpBufferCounter = 0;
            isJumping = false;
        }
    }
    /// <summary>
    /// Triggers the particle sytem to play/stop based on moving and grounded.
    /// </summary>
    private void PlayParticlesOnMoveOnGround()
    {
        if (inputX != 0 && isGrounded && canMakeDust)
        {
            footEmmision.rateOverDistance = footstepRateOverdistance;
        }
        else
        {
            footEmmision.rateOverDistance = 0;
            
        }
    }
    /// <summary>
    /// Move the rigidbody2D on the X axis based on the inputX value stored in the StoreMoveInput method.
    /// </summary>
    private void HorizontalMovement()
    {
        theRB.velocity = new Vector2(inputX * moveSpeed, theRB.velocity.y);

        if(inputX == 0 || !canMove)
        {
            
            playerAnimator.SetBool("IsMoving", false);
            footsteps.canPlayFootsteps = false;
        }
        else
        {
            if (isGrounded && canMove)
            {
                footsteps.canPlayFootsteps = true;
            }
            
            playerAnimator.SetBool("IsMoving", true);
        }
    }
    /// <summary>
    /// Clamping the fall speed to a value and smoothly lerping toward that value until the player
    /// is grounded again once the player is grounded again the camera speed is set back to the cache taken at the start
    /// </summary>
    private void ClampFallSpeed()
    {
        if (theRB.velocity.y < (maxFallSpeed * (-1)))
        {
            theRB.velocity = new Vector2(theRB.velocity.x, -maxFallSpeed);
            float speed = CameraController.instance.speedSmooth;
            ChangeSpeedOfCamSwitch(Mathf.Lerp(speed, speed * cameraFallSpeedMultiplier, fallSpeedSmooth * Time.deltaTime));
        }
        else
        {
            isRegisteredAsNotFalling = true;
        }
        if (isRegisteredAsNotFalling)
        {
            ChangeSpeedOfCamSwitch(smoothSpeedCache);
            isRegisteredAsNotFalling = false;
        }
    }

    private void IsOnWayDown()
    {
        if (theRB.velocity.y< 0.1f)
        {
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsFalling", true);
        }
    }
    /// <summary>
    /// Storing the values of our input on both the X & Y axes. Should be called from the Player Input component on the player object. 
    /// </summary>
    /// <param name="context"></param>
    public void StoreMoveInput(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
    }
    /// <summary>
    /// Should be called from the Player Input component on the player object. When correctly referenced: records the input state of the assigned Jump button
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            if (context.started) // Once the Jump input has been started and (the player is grounded or the hangcounter is above 0: meaning that coyote time is still in play. 
            {
                FirstJump();
            }
            else if (context.canceled)
            {
                if(theRB.velocity.y > 0)
                {
                    coyoteTimeCounter = 0f;
                    theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * (DownforcePostRelease / 100));
                }
            }
        }
    }
    public void UpdateRootbeerAmountAndUI()
    {
        if (RootBeerAmount <3)
        {

          
                RootBeerAmount++;
            
            
            if(RootBeerAmount ==1)
            {
                RootbeerUI.SetTrigger("isOneThird");
            }
            if(RootBeerAmount == 2)
            {
                RootbeerUI.SetTrigger("isTwoThirds");
            }
            if (RootBeerAmount == 3)
            {
                RootbeerUI.SetTrigger("isFull");
            }

        }else
        {
            RootBeerAmount = 3;
        }

        
        
        
    }
    
    public void Root(InputAction.CallbackContext context)
    {
        if(context.performed && cooldownToRootAbilityCounter <=0 && isGrounded && RootBeerAmount >= 3)
        {
            PlayEarthQuakeClip();
            RootBeerAmount = 0;
            PlayerHealthController.instance.HealPlayer();
            isRooting = true;
            theRB.velocity = new Vector2(0, theRB.velocity.y);
            Debug.Log("Rooting");
            playerAnimator.SetTrigger("IsRooted");
            RootbeerUI.SetTrigger("isDraining");
            vulneableTimeCountdown = vulnerableTime;
            //canMove = false;
            cooldownToRootAbilityCounter = cooldownToRootAbility;
            if (ZoneNumber == 1)
            {
                if (facingLeft)
                {
                    leftWallFar.SetTrigger("IsRootWall");
                }
                else
                {
                    leftWall.SetTrigger("IsRootWall");
                }
            }else if (ZoneNumber ==2)
            {
                if (facingLeft)
                {
                    leftWall.SetTrigger("IsRootWall");
                }
                else
                {
                    rightWall.SetTrigger("IsRootWall");
                }
            }
            else if (ZoneNumber ==3)
            {
                if (facingLeft)
                {
                    rightWall.SetTrigger("IsRootWall");
                }
                else
                {
                    rightWallFar.SetTrigger("IsRootWall");
                }
            }
            
            
        }
       
    }


    /// <summary>
    /// First jump logic. Takes into account Coyote Time and Jump Buffer, 
    /// </summary>
    private void FirstJump()
    {
        isJumping = true;
        jumpBufferCounter = jumpBufferTime; // Resets the jumpCounter (private) to the assigned public float jumpbufferLength.
            
    }
   
   
    /// <summary>
    /// Checks for collisions on the ground using the groundPoint, groundCheckRadius & whatisGround variables.
    /// </summary>
    public void CheckCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundCheckRadius, whatisGround);
    }
    /// <summary>
    /// switches isDashing to true, sets the particle systems to be active, registers the direction the player is facing, and begins the coroutine StopDashingRoutine which stops the dashing protocol.
    /// </summary>
    
    /// <summary>
    /// Sets the knockback counter and ditributed knockbackforce to the Rigidbody2D in the x axis & shakes camera.
    /// </summary>
    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        theRB.velocity = new Vector2(0f, knockBackForce);
        ShakeCamera(cameraShakeLength, cameraShakePower);
    }
    /// <summary>
    /// applies force (bounceForce) to the RigidBody2D in the y axis.
    /// </summary>
    public void Bounce()
    {
        ShakeCamera(cameraShakeLength, cameraShakePower);
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
    }
   
    /// <summary>
    /// If debug bool is enabled this method will print a message to the console
    /// </summary>
    /// <param name="message"></param>
    
    /// <summary>
    /// Shakes the camera by the given length (seconds),and power.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="power"></param>
    public void ShakeCamera(float length, float power)
    {
            ScreenShakeController.instance.StartShake(length, power);
    }
    
    // Drawing Our Raycasts so we can see them in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundPoint.position, groundCheckRadius);
    }
    /// <summary>
    /// Changes the speed of the follow cam by accessing the cameraController's speedSmooth variable and changing it to the changetoSpeed 
    /// </summary>
    /// <param name="changetoSpeed"></param>
    public void ChangeSpeedOfCamSwitch(float changetoSpeed)
    {
        CameraController.instance.speedSmooth = changetoSpeed;
    }
    /// <summary>
    /// Flips the local scale to give the impression that the player is turned to the right.
    /// </summary>
    public void FlipSpriteRight()
    {
        if (localScaleAtStart.x > 0)
        {
            transform.localScale = new Vector3(-localScaleAtStart.x, localScaleAtStart.y, localScaleAtStart.z);
        }
        else
        {
            transform.localScale = localScaleAtStart;
        }
            
        facingLeft = false;
    }
    /// <summary>
    /// Flips the local scale to give the impression that the player is turned to the left.
    /// </summary>
    public void FlipSpriteLeft()
    {
        if(localScaleAtStart.x > 0)
        {
            transform.localScale = new Vector3(localScaleAtStart.x, localScaleAtStart.y, localScaleAtStart.z);
        }
        else
        {
            transform.localScale = new Vector3(-localScaleAtStart.x, localScaleAtStart.y, localScaleAtStart.z);
        }
        facingLeft = true;
    }
    public void ResetPlayer()
    {
        isJumping = false;
        ChangeSpeedOfCamSwitch(smoothSpeedCache);
        theRB.velocity = Vector2.zero;
        theRB.gravityScale = gravityAtStart;
    }

    public void PlayJumpClip()
    {
        JumpSource.clip = GetRandomJumpClip();
        JumpSource.Play();
    }

    private AudioClip GetRandomJumpClip()
    {
        int randomIndex = Random.Range(0, jump.Length);
        return jump[randomIndex];
    }



}




