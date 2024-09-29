using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

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

    //collision checks vars
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool bumpedHead;

    // jump vars
    public float VerticalVelocity { get; private set; }
    private bool isFalling;
    private bool isFastFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int usedJumps;

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
    }

    public override void OnUpdate()
    {
        CountTimers();
        JumpChecks();


        if (!playerState.isAttacking)
        {
        }
    }
    public override void OnFixedUpdate()
    {
       CollisionCheck();
        Jump();
        accel = inputManager.isGrounded ? pms.GroundAccel : pms.AirAccel;
        decel = inputManager.isGrounded ? pms.GroundDecel : pms.AirDecel;
    }


    public void MovePlayer(float moveX)
    {
        if (!playerState.isAttacking)
        {
            //if (moveX != 0)
            if((moveX > .1f || moveX < -.1f))
            {
                //move check
                TurnCheck(moveX);

                if(inputManager.isGrounded)
                {
                 if (inputManager.isRunning)
                 animController.Play(Animations.RUN, false, false);
                 else
                    animController.Play(Animations.WALK, false, false);
                }


                Vector2 targetVelocity = Vector2.zero;
                targetVelocity = new Vector2(moveX, 0f) * (inputManager.isRunning ? pms.MaxRunSpeed : pms.MaxWalkSpeed);
                moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, accel * Time.fixedDeltaTime);
                rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);

            }
            else if (!(moveX > .1f || moveX < -.1f))
            {
                if(inputManager.isGrounded)
                animController.Play(Animations.IDLE, false, false);

                moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
                rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
            }


            if(!inputManager.isGrounded)
            {
                if(rb.velocity.y < 0.6f)
                animController.Play(Animations.FALLING, false, false);
                else if (usedJumps == 1)
                animController.Play(Animations.SINGLE_JUMP, false, false);
                else
                animController.Play(Animations.DOUBLE_JUMP, false, false);

            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void TurnCheck(float moveX)
    {
        if (isRight && moveX < 0)
            Turn(false);
        if (!isRight && moveX > 0)
            Turn(true);
    }

    private void Turn(bool turn)
    {
        isRight = turn;
        transform.Rotate(0f,180f *(turn ? 1f : -1f) ,0f);
    }


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;


        inputManager.OnMove+=MovePlayer;
    }

    private void isGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feet.bounds.center.x, feet.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feet.bounds.size.x, pms.GroundDetectLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin,boxCastSize,0f,Vector2.down, pms.GroundDetectLength,pms.GroundLayer);
        inputManager.isGrounded = groundHit.collider != null ? true : false;

        if(pms.ShowGroundBox)
        {
            Color rayColor;
            rayColor = inputManager.isGrounded ? Color.green : Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x/2, boxCastOrigin.y),Vector2.down * pms.GroundDetectLength,rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x/2, boxCastOrigin.y),Vector2.down * pms.GroundDetectLength,rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x/2, boxCastOrigin.y - pms.GroundDetectLength),Vector2.right * boxCastSize.x, rayColor);

        }
    }

    private void bumpHead()
    {
        Vector2 boxCastOrigin = new Vector2(feet.bounds.center.x, body.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feet.bounds.size.x * pms.HeadWidth, pms.HeadDetectLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, pms.HeadDetectLength, pms.GroundLayer);
        bumpedHead = groundHit.collider != null ? true : false;
    }

    private void CollisionCheck()
    {
        isGrounded();
        bumpHead();
    }

    private void JumpChecks()
    {
        //PRESSING JUMP
        if(inputManager.JumpPressed)
        {
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
        if(jumpBufferTimer > 0f && !inputManager.isJumping && (inputManager.isGrounded || coyoteTimer > 0f))
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
        if((inputManager.isJumping || isFalling) && inputManager.isGrounded && VerticalVelocity <= 0f)
        {
            inputManager.isJumping = isFalling = isFastFalling = isPastApexThreshold = false;
            fastFallTime = usedJumps = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int jumpsUsed)
    {
        if (!inputManager.isJumping)
            inputManager.isJumping = true;

        jumpBufferTimer = 0f;
        usedJumps += jumpsUsed;
        VerticalVelocity = pms.InitialJumpVelo;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if(inputManager.isJumping)
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
                    VerticalVelocity = timePastApexThreshold < pms.ApexhangTime ? 0f : -0.01f;
                }
            }

            //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
            else
            {
             VerticalVelocity += pms.Gravity * Time.fixedDeltaTime;
             if (isPastApexThreshold)
                isPastApexThreshold = false;
            }


         }
         //GRAVITY ON DESCENDING
         else if (!isFastFalling)
            VerticalVelocity += pms.Gravity * pms.GravityReleaseMultiplyer * Time.fixedDeltaTime;
         else if (VerticalVelocity < 0f)
            if (!isFalling)
                isFalling = true; 
        }

        //JUMP CUT
        if(isFastFalling)
        {
            VerticalVelocity = fastFallTime >= pms.UpCancelTime ? VerticalVelocity + (pms.Gravity * pms.GravityReleaseMultiplyer * Time.fixedDeltaTime) : Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / pms.UpCancelTime));
            fastFallTime += Time.fixedDeltaTime;
        }

        // NORMAL GRAVITY WHILE FALLING
        if(!inputManager.isGrounded && !inputManager.isJumping)
        {
            if (!isFalling)
                isFalling = true;

            VerticalVelocity += pms.Gravity * Time.fixedDeltaTime;
        }

        //CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -pms.MaxFallSpeed, 50f);
        rb.velocity = new Vector2(rb.velocity.x, VerticalVelocity);
    }

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer = inputManager.isGrounded ? pms.JumpCoyoteTime : coyoteTimer - Time.deltaTime;
    }

    #region JUMPARC

    private void DrawJumpArc(float moveSpeed , Color gizmoColor)
    {
        Vector2 startpos = new Vector2(feet.bounds.center.x, feet.bounds.min.y);
        Vector2 prevpos = startpos;
        float speed = 0f;

        speed = pms.DrawRight ? moveSpeed : -moveSpeed;

        Vector2 velocity = new Vector2(speed, pms.InitialJumpVelo);
        Gizmos.color = gizmoColor;

        float timestep = 2 * pms.JumpApexTime / pms.ArcRes;

        for(int i = 0; i < pms.VisualSteps; i++)
        {
            float simtime = i * timestep;
            Vector2 displacement;
            Vector2 drawpoint;

            if (simtime < pms.JumpApexTime)
                displacement = velocity * simtime + 0.5f * new Vector2(0, pms.Gravity) * simtime * simtime;
            else if(simtime < pms.JumpApexTime +pms.ApexhangTime)
            {
                float apexTime = simtime - pms.JumpApexTime;
                displacement = velocity * pms.JumpApexTime + 0.5f * new Vector2(0, pms.Gravity) * pms.JumpApexTime * pms.JumpApexTime;
                displacement += new Vector2(speed, 0) * apexPoint;
            }
            else
            {
                float descendtime = simtime - (pms.JumpApexTime + pms.ApexhangTime);
                displacement = velocity * pms.JumpApexTime +0.5f * new Vector2(0, pms.Gravity) * pms.JumpApexTime * pms.JumpApexTime;
                displacement += new Vector2(speed, 0) * pms.ApexhangTime;
                displacement += new Vector2(speed, 0) * descendtime + 0.5f * new Vector2(0, pms.Gravity) * descendtime * descendtime;
            }
            drawpoint = startpos + displacement;

            if(pms.StopOncollision)
            {
                RaycastHit2D hit = Physics2D.Raycast(prevpos, drawpoint - prevpos, Vector2.Distance(prevpos, drawpoint), pms.GroundLayer);
                if(hit.collider != null)
                {
                    Gizmos.DrawLine(prevpos, hit.point);
                    break;
                }
            }

            Gizmos.DrawLine(prevpos, drawpoint);
            prevpos = drawpoint;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (pms.WalkJumpArc)
            DrawJumpArc(pms.MaxWalkSpeed, Color.red);
        if (pms.RunJumpArc)
            DrawJumpArc(pms.MaxRunSpeed, Color.green);
    }
}
