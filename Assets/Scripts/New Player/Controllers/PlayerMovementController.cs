using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerComponent
{
    [Header("Refrences")]
    public PlayerMovementStats pms;
    [SerializeField] private Collider2D feet;
    [SerializeField] private Collider2D body;

    private Rigidbody2D rb;

    // move vars
    private Vector2 moveVelocity;
    private bool isRight;
    private float accel;
    private float decel;
    private float dir;
    [Range(0f,1.2f)]
    public float airfloat = .7f;


    // jump vars
    public float VerticalVelocity { get; private set; }
    private bool isFalling;
    private bool isFastFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    public int usedJumps;
    public bool bumpedHead;
    public bool SSpecialFall = false;

    //apex vars
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //jump buffer vars
    private float jumpBufferTimer;
    private bool jumpReleaseDuringBuffer;

    //coyote tume vars
    private float coyoteTimer;


    public override void OnStart()
    {
        print("Hello PlayerMovementController!");
        rb = GetComponent<Rigidbody2D>();
        isRight = true;
        playerState.CanUSpecial = true;
        playerState.CanSSpecial = true;

}

public override void OnUpdate()
    {
        if (playerState.isInBB)
        {
            rb.velocity *= 2.0f;
        }

        SendDataToPlayerState();
        if(!playerState.isSSpecial)
        CountTimers();
        if(!playerState.isUSpecial && inputManager.canControl)
        JumpChecks();
    }
    public override void OnFixedUpdate()
    {
        if (inputManager.canControl)
        {
            if (playerState.isUSpecial)
                USpecialJump();

            if (inputManager.isJumping && !playerState.isUSpecial)
                Jump();


        }

        if (rb.velocity.y > 1)
            playerState.CanLedgeGrab = true;

        if (!playerState.isLedgeGrab && ( playerState.VelocityStunned || inputManager.canControl))
                Falling();

        accel = playerState.isGrounded ? pms.GroundAccel : pms.AirAccel;
        decel = playerState.isGrounded ? pms.GroundDecel : pms.AirDecel;
    }


    private void Falling()
    {
        if (playerState.isAriel)
            VerticalVelocity *= airfloat;

        if (isFastFalling && !playerState.isUSpecial && !playerState.VelocityStunned)
        {

            VerticalVelocity = fastFallTime >= pms.UpCancelTime ? VerticalVelocity + ((playerState.isInBB ? 2.0f : 1.0f) *  (pms.Gravity * pms.GravityReleaseMultiplyer * Time.fixedDeltaTime)) : Mathf.Lerp((playerState.isInBB ? 2.0f : 1.0f) * fastFallReleaseSpeed, 0f, (playerState.isInBB ? 2.0f : 1.0f) * (fastFallTime / pms.UpCancelTime));
            fastFallTime += Time.fixedDeltaTime;
            
        }

        // NORMAL GRAVITY WHILE FALLING
        if (!playerState.isGrounded && !inputManager.isJumping && !playerState.isUSpecial && !playerState.VelocityStunned)
        {
                print("RAHHHHHHHHH2H");

            if (!isFalling)
                isFalling = true;

                VerticalVelocity += (playerState.isInBB ? 2.0f : 1.0f) * (pms.Gravity * Time.fixedDeltaTime);
        }

        if (!playerState.isSSpecial && !playerState.isDSpecial && !pms.dashAttack && !playerState.VelocityStunned)
        {
            VerticalVelocity = Mathf.Clamp(VerticalVelocity, (playerState.isInBB ? 2.0f : 1.0f) * -pms.MaxFallSpeed, (playerState.isInBB ? 2.0f : 1.0f) * 50f);

            //CLAMP FALL SPEED
            if (playerState.isUSpecial)
                rb.velocity = new Vector2(rb.velocity.x, (playerState.isInBB ? 2.0f : 1.0f) * VerticalVelocity);
            else
                rb.velocity = new Vector2(rb.velocity.x, (playerState.isInBB ? 2.0f : 1.0f) * VerticalVelocity);
        }
        #region Specials Velo
        
                else if (playerState.isSSpecial)
                {
                    int SSpecialDir =  (int)(dir != 0 ? (dir > 0 ? 1 : -1) : (transform.eulerAngles.y == 0 ? 1f : -1f));
                    rb.velocity = new Vector2(pms.HorizontalSlideVelo * SSpecialDir, 0f);
                }
                else if (playerState.isDSpecial)
                {
                    rb.velocity = new Vector2(pms.dosnSpecialHorizontalVelocity * dir , -9.81f);
                }
                else if (pms.dashAttack)
                {
                    if (rb.velocity.x >= 0)
                        rb.velocity -= new Vector2(pms.slowdownVelocity * dir, 0f);
                }
                else if (playerState.VelocityStunned)
                {
                    if (playerState.isAttacking)
                        print("RAHHHHHHHHH4H");

                    if (playerState.isInBB)
                        print("Blade Velo: " + rb.velocity);

                    rb.velocity += new Vector2(0, (playerState.isInBB ? 2.0f : 1.0f) * (pms.Gravity * Time.fixedDeltaTime));

                }
         
        #endregion

    }

    #region Movement
    public void MovePlayer(float moveX , float moveY)
    {
        if (!playerState.isSSpecial && !pms.dashAttack)
        { 

                if (moveX > .1f || moveX < -.1f)
                {
                    dir = moveX;
                }
                else
                {
                    dir = (!playerState.isGrounded && moveX == 0 && moveY > -.2f && !pms.Flatmove) ? .5f * (isRight ? 1 : -1) : 0;
                }

             if (!playerState.isLedgeGrab)
             {
                if (!playerState.isAttacking)
                {
                    if (dir > .1f || dir < -.1f)
                    {
                        //move check
                        TurnCheck(dir);

                        if (playerState.isGrounded)
                        {
                            if (inputManager.isRunning)
                                animController.Play(Animations.RUN, false, false);
                            else
                                animController.Play(Animations.WALK, false, false);
                        }


                        Vector2 targetVelocity = Vector2.zero;
                        targetVelocity = new Vector2(dir, 0f) * (inputManager.isRunning ? pms.MaxRunSpeed : pms.MaxWalkSpeed);
                        moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, accel * Time.fixedDeltaTime);

                    }
                    else if (!(dir > .1f || dir < -.1f))
                    {
                        if (playerState.isGrounded && moveY > -.4f)
                            animController.Play(Animations.IDLE, false, false);
                        else if(playerState.isGrounded && moveY <= -.4f && !playerState.isOnPassThrough)
                            animController.Play(Animations.Crouch, false, false);

                        moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
                    }
                        if(!playerState.VelocityStunned)
                            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);


                    if (!playerState.isGrounded)
                    {
                        if (rb.velocity.y < -1.0f && !playerState.isLedgeGrab)
                        {

                            animController.Play(Animations.FALLING, false, false);
                        }
                        else if (usedJumps == 1 && !playerState.VelocityStunned)
                            animController.Play(Animations.SINGLE_JUMP, false, false);
                        else if (usedJumps > 1)
                        {
                            animController.Play(Animations.DOUBLE_JUMP, false, false);
                        }

                    }
                }
                else if (playerState.isUSpecial)
                {
                    rb.velocity = new Vector2(pms.USpecHorfact * (isRight ? 1 : -1), rb.velocity.y);
                }
                else if (playerState.isDSpecial)
                {
                    rb.velocity = new Vector2(pms.dosnSpecialHorizontalVelocity * (dir), rb.velocity.y);
                }
                else if (pms.dashAttack)
                {
                    if (rb.velocity.x >= 0)
                        rb.velocity -= new Vector2(pms.slowdownVelocity * (dir), rb.velocity.y);
                }
                else if (playerState.isAriel)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }
                else if (!playerState.VelocityStunned)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
             }
        }
    }



    private void TurnCheck(float moveX)
    {
        if (playerState.isGrounded)
        {
            if (isRight && moveX < 0)
                Turn(false);
            if (!isRight && moveX > 0)
                Turn(true);
        }
    }

    public void Turn(bool turn)
    {
        isRight = turn;
        transform.Rotate(0f,180f *(turn ? 1f : -1f) ,0f);
    }

    #endregion


    private void JumpChecks()
    {
        //PRESSING JUMP
        if (inputManager.JumpPressed)
        {
            playerState.isLedgeGrab = false;
            jumpBufferTimer = pms.JumpBufferTime;
            jumpReleaseDuringBuffer = false;
        }


        //RELEASING JUMP
        if(inputManager.JumpReleased)
        {
            if (jumpBufferTimer > 0f)
                jumpReleaseDuringBuffer = true;

            if(inputManager.isJumping && VerticalVelocity > 0f)
                if(isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = pms.UpCancelTime;
                    VerticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = VerticalVelocity;
                }
        }

        //jUMP WITH JUMP BUFFER AND COYOTE
        if(jumpBufferTimer > 0f && !inputManager.isJumping && (playerState.isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if(jumpReleaseDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = VerticalVelocity;

            }
        }

        //DOUBLE JUMP
        else if (jumpBufferTimer > 0f && inputManager.isJumping && usedJumps < pms.NumberofJumps)
        {
            isFastFalling = false;
            InitiateJump(1);
        }

        //AIR JUMP AFTER COYOTE
        else if (jumpBufferTimer > 0f && isFalling && usedJumps < pms.NumberofJumps -1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }

        //LANDED
        if((inputManager.isJumping || isFalling) && playerState.isGrounded && VerticalVelocity <= 0f)
        {
            inputManager.isJumping = isFalling = isFastFalling = isPastApexThreshold = false;
            fastFallTime = usedJumps = 0;

            VerticalVelocity = (playerState.isInBB ? 2.0f : 1.0f) * Physics2D.gravity.y;
        }

    }

    private void InitiateJump(int jumpsUsed)
    {
        if (playerState.isGrounded || playerState.isLedgeGrab)
            pms.Flatmove = dir == 0 ? true : false;

        if (!inputManager.isJumping)
            inputManager.isJumping = true;

        jumpBufferTimer = 0f;
        usedJumps += jumpsUsed;
        VerticalVelocity = (playerState.isInBB ? 1.0f : 1.0f) * pms.InitialJumpVelo;
    }

    private void Jump()
    {
         //CHECK FOR HEAD BUMP
         if (bumpedHead)
                isFastFalling = true;
        
         //GRAVITY ON ASCENDING
         if(VerticalVelocity >=0f)
         {
            //APEX CONTROLS
            apexPoint = Mathf.InverseLerp(pms.InitialJumpVelo, 0f, VerticalVelocity);

            if(apexPoint > pms.ApexThreshold)
            {
                if(!isPastApexThreshold)
                {
                    isPastApexThreshold = true;
                    timePastApexThreshold = 0;
                }
                if(isPastApexThreshold)
                {
                    timePastApexThreshold += Time.fixedDeltaTime;
                    VerticalVelocity = (playerState.isInBB ? 2.0f : 1.0f) * timePastApexThreshold < pms.ApexhangTime ? 0f : -0.01f;
                }
            }

            //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
            else
            {
             VerticalVelocity += (playerState.isInBB ? 2.0f : 1.0f) *  (pms.Gravity * Time.fixedDeltaTime);
             if (isPastApexThreshold)
                isPastApexThreshold = false;
            }


         }
         //GRAVITY ON DESCENDING
         else if (SSpecialFall)
            VerticalVelocity += (playerState.isInBB ? 2.0f : 1.0f) * (pms.Gravity * pms.SSpecialFallMulti * Time.fixedDeltaTime);
         else if (!isFastFalling)
            VerticalVelocity += (playerState.isInBB ? 2.0f : 1.0f) * (pms.Gravity * pms.GravityReleaseMultiplyer * Time.fixedDeltaTime);
         else if (VerticalVelocity < 0f)
            if (!isFalling)
                isFalling = true; 
      
    }
    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer = playerState.isGrounded ? pms.JumpCoyoteTime : coyoteTimer - Time.deltaTime;
    }

    public void DashLaunch()
    {
        pms.dashAttack = !pms.dashAttack;
    }
        #region Ledges

    public void LedgeGrabStop()
    {
       rb.velocity = Vector2.zero;
       VerticalVelocity = moveVelocity.x = 0f;
       usedJumps = usedJumps == 2 ? 1 : usedJumps;
    }

        #endregion
   
    #region SpecailVelo

    private void USpecialJump()
    {
        //CHECK FOR HEAD BUMP
        if (bumpedHead)
            isFastFalling = true;

        //GRAVITY ON ASCENDING
        if (VerticalVelocity >= 0f)
        {
            //APEX CONTROLS
            apexPoint = Mathf.InverseLerp(pms.USpecInitialJumpVelo, 0f, VerticalVelocity);

            if (apexPoint > pms.ApexThreshold)
            {
                if (!isPastApexThreshold)
                {
                    isPastApexThreshold = true;
                    timePastApexThreshold = 0;
                }
                if (isPastApexThreshold)
                {
                    timePastApexThreshold += Time.fixedDeltaTime;
                    VerticalVelocity = (playerState.isInBB ? 2.0f : 1.0f) * timePastApexThreshold < pms.ApexhangTime ? 0f : -0.01f;
                }
            }

            //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
            else
            {
                VerticalVelocity += (playerState.isInBB ? 2.0f : 1.0f) *(pms.Gravity * Time.fixedDeltaTime);
                if (isPastApexThreshold)
                    isPastApexThreshold = false;
            }


        }
    }

    public void USpecialLaunch()
    {
            isFastFalling = isFalling = false;
            jumpBufferTimer = 0f;
            usedJumps += 2;
            VerticalVelocity = pms.USpecInitialJumpVelo;
            rb.velocity = new Vector2(rb.velocity.x, VerticalVelocity);
    }

    public void SSpecialLaunch()
    {
        isFastFalling =  false;
        rb.velocity = new Vector2(0, 0);
        SSpecialFall = true;
    }
    #endregion


    private void SendDataToPlayerState()
    {
        playerState.isRight = isRight;
        playerState.isRunning = inputManager.isRunning;
    }


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);

        inputManager.OnMove += MovePlayer;
        EventManager.onCutesceneEnter.AddListener(ConfigureForCutscene);
    }

    public void ApplyKnockback(float angle , float Kb)
    {
        print("Angle: "+ angle);
        float healthWeight = 0;
        float stunMultiplier = 10 / playerState.MaxHealth;

        if (playerState.Health > 0)
        {
            healthWeight = ((playerState.MaxHealth / playerState.Health) * stunMultiplier);

        }
        else healthWeight = ((playerState.MaxHealth / 1f) * stunMultiplier);
        angle += playerState.Health < .5 ? 0 : 10;
        float XComponent = Mathf.Cos(angle * Mathf.Deg2Rad) * Kb;
        float YComponent = Mathf.Sin(angle * Mathf.Deg2Rad) * Kb;

        
        if (playerState.isRight)
        {
            XComponent *= -1;
        }
        rb.velocity = Vector2.zero;
        rb.velocity = (new Vector2(XComponent, YComponent) * healthWeight );

        print(rb.velocity);

    }



    public void ConfigureForCutscene()
    {
        rb.velocity = Vector2.zero;
    }

    public void Interpolate(bool b)
    {
        rb.interpolation =  b ? RigidbodyInterpolation2D.Interpolate : RigidbodyInterpolation2D.None;
    }
}
