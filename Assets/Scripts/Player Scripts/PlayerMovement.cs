using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    private Collider2D playerCollider;
    public Animator anim;
    public Transform cam;
    public Vector3 offset;

    public float Walk, jumpForce, fallingGravityFactor;
    public float Run, Crawl, scaledGravity, jumpAmt;
    public float Speed;


    public Material crouchMat, RunMat;

    private GameObject currentPassThroughPlatform;

    [Header("Movement")]
    public bool isFalling = false;
    public bool isLeft = false;
    public bool isInAir;
    public bool isInLandingLag = false;
    public bool isOnPassThrough = false;
    private bool isCoroutineRunning = false;
    bool isCrouch = false;
    public float crouchTimer = .5f;
    public float terminalVelocityY = -10f;
    private Vector2 smoothVector;
    private Vector2 smoothVelocity;
    [Range(1,10)]
    public float DIFactor = 4f;
    private bool canApplyAirMovement = true;
    public bool isInGetup = false;


    public Transform[] groundRays;
    public float rayRange = 5f;
    public bool dashDisable = false;

    [Header("Attacks")]
    public PlayerAttack attackScript;
    public ShieldScript SS;

    public Stun S;
    private bool stunned;

    [Header("Escelator")]
    private Escelator Escelator;
    public bool OnEscelator, InEscelator = false;

    [Header("LedgeGrab")]
    public bool grabbing;
    LedgeGrab lg;

    Health H;

    float globalDirX;

    public int curDamage = 0;

    public static PlayerMovement instance;
    public bool lockCam = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isInAir = true;
        ChangeHealth();

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        attackScript = GetComponent<PlayerAttack>();
        lg = GetComponent<LedgeGrab>();
        H = GetComponent<Health>();
            
        Speed = Walk;
        Run = Walk * 2;
        Crawl = Walk / 2;
        scaledGravity = rb.gravityScale * fallingGravityFactor;
        jumpAmt = 0;


        S = GetComponent<Stun>();

        if (terminalVelocityY > 0)
        {
            terminalVelocityY *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        globalDirX = Input.GetAxisRaw("Horizontal");

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpSqaut"))
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        stunned = S.getIsStunned();

        if (Escelator != null)
            OnEscelator = Escelator.GetOnEscelator();

        isInLandingLag = anim.GetCurrentAnimatorStateInfo(0).IsName("Fall 2 Idle");

        if (!lg.action && !H.dead && !lockCam)
            cam.position = this.transform.position + offset;

        isCrouch = anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch");

        if (Input.GetButton("Run") && !isInAir && !isCrouch && !dashDisable)
        {
            Speed = Run;
            anim.SetTrigger("Running");
            anim.ResetTrigger("Walking");

        }
        else if (dashDisable && Input.GetAxisRaw("Horizontal") != 0)
        {
            anim.SetTrigger("Walking");
            Speed = 9f;
        }

        else if (Input.GetAxisRaw("Vertical") < 0f && !isInAir && isCrouch)
        {
            Speed = Crawl;
            anim.ResetTrigger("Crouch");
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
        }
        else if (!Input.GetButton("Run") && !isInAir && !isCrouch)
        {
            Speed = Walk;
            anim.ResetTrigger("Running");
        }
        else
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            if (Escelator == null && !isInAir)
                anim.SetTrigger("Idle");

        }

        DetermineMovementStates();

        if (rb.velocity.y < terminalVelocityY)
        {
            rb.velocity = new Vector2(rb.velocity.x, terminalVelocityY);
        }
    }

    void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonUp("Run"))
        {
            dashDisable = false;
        }
        smoothVector = Vector2.SmoothDamp(smoothVector, new Vector2(dirX * Speed, 0.0f), ref smoothVelocity, .1f);



        //Additional logic for stunning and getting hit. Need to figure out way to improve it

        float xForce = 0;

        if (!dashDisable && canApplyAirMovement)
        {
             xForce = (dirX < 0 ? -Speed : Speed) / 2;


            if (dirX == 0 && isInAir && rb.velocity.x == 0 || !isInAir)
            {
                xForce = 0;
            }

            else if (dirX == 0 && isInAir && rb.velocity.x != 0)
            {
                xForce = (rb.velocity.x < 0 ? -Speed : Speed) / 2;
            }
        }
        else if(!canApplyAirMovement && dirX!=0)
        {
            canApplyAirMovement = !canApplyAirMovement;
        }

        if (!attackScript.bypassMoveBlock)
        {
            rb.velocity = new Vector2(isInLandingLag || (attackScript.isAttacking && !isInAir) || isCrouch || attackScript.isSpecial || anim.GetBool("hasGrabbedEnemy") ? 0 : (dirX > 0 ? dirX * smoothVector.x : -dirX * smoothVector.x)+ (xForce), rb.velocity.y);

  
          
          
        }

        else
        {
            Speed = Crawl;
            rb.velocity = new Vector2((dirX > 0 ? dirX * smoothVector.x : -dirX * smoothVector.x), rb.velocity.y);
        }

        if(rb.velocity.x > Speed)
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        else if(rb.velocity.x < -Speed)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
        }

        if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") >= 0f)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            anim.SetTrigger("Idle");
            isCrouch = false;
        }

        else if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") < 0f && anim.GetBool("Climbing") == false)
        {
            rb.gravityScale = 1;
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Crouch");
            isCrouch = true;
        }

        else
        {
            if (dirX != 0 && !attackScript.isAttacking && !isInAir && !anim.GetBool("hasGrabbedEnemy"))
                transform.eulerAngles = new Vector2(0, dirX < 0 ? 180 : 0);

            isLeft = transform.eulerAngles.y == 0 ? false : true;

            if (!isInAir && !anim.GetBool("OnLadder"))
            {
                switch (Speed)
                {
                    case 14f:
                        //isCrouch = false;
                        anim.ResetTrigger("Walking");
                        break;

                    case 7f:
                        // isCrouch = false;
                        anim.SetTrigger("Walking");
                        break;
                    case 3.5f:
                        //isCrouch = true;
                        anim.ResetTrigger("Walking");
                        break;


                }
                anim.ResetTrigger("Crouch");
                anim.ResetTrigger("Idle");
            }
        }

    }

    void Jump()
    {
        if (!(jumpAmt == 0 && isInAir && !anim.GetBool("Climbing")) && !isInLandingLag && !attackScript.isAttacking && !anim.GetBool("hasGrabbedEnemy"))
        {
            if (anim.GetBool("Climbing"))
                jumpAmt = 0;

            if (Input.GetButtonDown("Jump") && !anim.GetAnimatorTransitionInfo(0).IsName("Idle -> Crouch"))
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Climbing");
                isInAir = true;

                anim.SetBool("isGrounded", !isInAir);
                if (jumpAmt < 2)
                {

                    jumpAmt++;
                    switch (jumpAmt)
                    {
                        case 1:
                            if (isInLandingLag == false)
                                anim.SetBool("isJumping", true);
                            anim.SetBool("isDoubleJumping", false);
                            break;
                        case 2:
                            anim.SetBool("isJumping", false);
                            anim.SetBool("isDoubleJumping", true);
                            rb.gravityScale = 1.0f;
                            break;
                    }

                    if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Crouch")
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        anim.SetBool("isFalling", false);
                    }

                }
            } 




            else if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, isFalling ? rb.velocity.y : rb.velocity.y / 2);
            }
        }



        if (rb.velocity.y < 0.0f && Escelator == null)
        {
            if (!InEscelator)
                isFalling = true;
            //Debug.Log(rb.velocity.y + "  " + isFalling);

            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isGrounded", !isFalling);
            anim.ResetTrigger("Crouch");
            if (!PlayerAttack.attackInstance.SideBS)
                rb.gravityScale = scaledGravity;
        }
        else isFalling = false;


        anim.SetBool("isFalling", isFalling);

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (currentPassThroughPlatform != null && !isCoroutineRunning)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }


    private void DetermineMovementStates()
    {
        if (!stunned && !anim.GetBool("Taunt") && !grabbing && !S.isAirSpin && !anim.GetBool("isAirStunned") && !anim.GetBool("isLaying") && !H.dead && !SS.ShieldStun && !isInGetup)
        {

            if (isInAir)
                Speed = Walk;

            if (!anim.GetBool("Climbing") && !anim.GetBool("isAirStunned") && !anim.GetBool("isLaying"))
                Move();            

            if (!PlayerAttack.attackInstance.isExecutedOnce)
                Jump();

            anim.SetBool("isGrounded", !isInAir);

            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", isFalling);
            }
            else anim.SetBool("isFalling", isInAir);
        }

        else if (!stunned && S.isAirSpin)
        {
            if (globalDirX != 0 && S.isAirSpin)
            {
                S.isAirSpin = false;
                anim.SetBool("isAirStunned", S.isAirSpin);

            }
        }

        else if (!stunned)
        {
            if (globalDirX != 0)
            {
                anim.SetBool("isLaying", false);
            }

            else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButton("Fire3"))
            {
                anim.SetBool("canLayAttack", false);
            }
        }


        else if (stunned)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            if (rb.velocity.y < 0.0f)
            {
                isFalling = true;
                anim.SetBool("isJumping", false);
                anim.SetBool("isDoubleJumping", false);
                anim.SetBool("isGrounded", !isInAir);
                anim.ResetTrigger("Crouch");
                rb.gravityScale = scaledGravity;
            }
            else isFalling = false;
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform" || collision.gameObject.tag == "MovingPlatform") && collision.gameObject.tag != "Wall")
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("USpecial") && collision.gameObject.tag == "PassThroughPlatform")
            {
                //Do nothing
            }
            else
                Physics2D.gravity = new Vector2(0, -9.81f);

            if (collision.gameObject.GetComponent<Escelator>())
                Escelator = collision.gameObject.GetComponent<Escelator>();

            if (anim.GetBool("Climbing"))
                anim.ResetTrigger("Climbing");


            rb.gravityScale /= rb.gravityScale == scaledGravity ? scaledGravity : 1.0f;

            if (rb.velocity.y == 0 || Escelator != null )
                isInAir = false;



            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[0].transform.position, -Vector2.up * rayRange);
            Debug.DrawRay(groundRays[0].position, -Vector2.up * rayRange);

            if (collision.gameObject.tag == "PassThroughPlatform")
            {

                if (!attackScript.isAttacking && rb.velocity.y == 0)
                {
                    anim.SetBool("isFalling", false);
                    jumpAmt = 0;
                    PlayerAttack.attackInstance.isExecutedOnce = false;

                }

                isOnPassThrough = true;

                if (hitGround.collider.tag == "PassThroughPlatform" && !isInAir )
                {
                    jumpAmt = 0;
                }
                else jumpAmt = 0;
                currentPassThroughPlatform = collision.gameObject;
                anim.SetBool("isGrounded", isOnPassThrough);
            }

            if ((collision.gameObject.tag == "Platform" || collision.gameObject.tag == "MovingPlatform") && !grabbing && !isInAir)
            {
                jumpAmt = 0;
                PlayerAttack.attackInstance.isExecutedOnce = false;
                isOnPassThrough = false;
            }

            if(S.isAirSpin)
            {
                stunned = false;
                anim.SetBool("Stunned", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isGrounded", true);
            }
            else anim.SetBool("isGrounded", true);

        }

        if (attackScript.isInHelpless && rb.velocity.y == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            attackScript.isInHelpless = !attackScript.isInHelpless;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PassThroughPlatform" || collision.gameObject.tag == "Platform")
        {
            if (!collision.gameObject.GetComponent<Escelator>())
            {
                Escelator = null;
                OnEscelator = false;

                if (Input.GetButton("Jump"))
                {
                    anim.ResetTrigger("Crouch");
                    anim.ResetTrigger("Idle");
                    isInAir = true;
                    anim.SetBool("isGrounded", !isInAir);
                    anim.SetBool("isJumping", true);
                    anim.SetBool("isDoubleJumping", false);
                    anim.SetBool("isFalling", false);
                }
            }

            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[1].transform.position, -Vector2.up * rayRange);
            if (!isInAir && collision.gameObject.tag == "Platform" && hitGround.collider.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform" && hitGround.collider.tag == "PassThroughPlatform")
            {
                if (collision.gameObject.transform.position.y < transform.position.y)
                    jumpAmt = 1;
            }
            currentPassThroughPlatform = null;

            if (anim.GetBool("Climbing"))
                isInAir = true;

            if (rb.velocity.y != 0)
                isInAir = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Escelator>() && rb.velocity.y < 0f)
        {
            InEscelator = true;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            Speed = Walk;
            if (isInAir)
                jumpAmt = 1;
        }

        if (collision.gameObject.GetComponent<Escelator>())
        {
            InEscelator = false;
        }
    }


    private IEnumerator DisableCollision()
    {
        isCoroutineRunning = true;
        BoxCollider2D platformCol = currentPassThroughPlatform.GetComponent<BoxCollider2D>();
        yield return new WaitForSeconds(crouchTimer);
        if (!attackScript.isAttacking && isOnPassThrough)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCol);
            yield return new WaitForSeconds(.5f);
            Physics2D.IgnoreCollision(playerCollider, platformCol, false);
        }
        else isOnPassThrough = true;
        isCoroutineRunning = false;

    }


    //ACCESSORS AND MUTATORS

    public bool GetIsLeft()
    {
        return isLeft;
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    public float GetScaledGravity()
    {
        return scaledGravity;
    }

    public bool GetIsInAir()
    {
        return isInAir;
    }

    public bool GetIsStunned()
    {
        return stunned;
    }

    public void SetIsOnPassThrough(bool newPT)
    {
        isOnPassThrough = newPT;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    public void ChangeHealth()
    {

    }

 

    public bool getCanApplyAirMovement()
    {
        return canApplyAirMovement;
    }

    public void setcanApplyAirMovement(bool canApplyAirMovement)
    {
        this.canApplyAirMovement = canApplyAirMovement;
    }

}