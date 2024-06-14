using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    public PlayerRefrecnces PR;

    [Header("Components")]
    public SpriteRenderer sr;
    private Collider2D playerCollider;

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
    public ShieldScript SS;


    [Header("Escelator")]
    private Escelator Escelator;
    public bool OnEscelator, InEscelator = false;

    Health H;

    private bool OnMoveable = false;

    float globalDirX;

    public int curDamage = 0;

    public bool isInCutscene = false;

    private GameObjectToDoorInterface gameObjectToDoorInterface;

    // Start is called before the first frame update
    void Start()
    {
        isInAir = true;
        ChangeHealth();


        playerCollider = GetComponent<Collider2D>();
        H = GetComponent<Health>();
        gameObjectToDoorInterface = GetComponent<GameObjectToDoorInterface>();

            
        Speed = Walk;
        Run = Walk * 2;
        Crawl = Walk / 2;
        scaledGravity = PR.RB.gravityScale * fallingGravityFactor;
        jumpAmt = 0;



        if (terminalVelocityY > 0)
        {
            terminalVelocityY *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInCutscene)
        {
            globalDirX = Input.GetAxisRaw("Horizontal");

            if (PR.anim.GetCurrentAnimatorStateInfo(0).IsName("JumpSqaut"))
                PR.RB.velocity = new Vector2(PR.RB.velocity.x, jumpForce);


            if (Escelator != null)
                OnEscelator = Escelator.GetOnEscelator();

            isInLandingLag = PR.anim.GetCurrentAnimatorStateInfo(0).IsName("Fall 2 Idle");

            isCrouch = PR.anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch");

            if (Input.GetButtonDown("DoorEnter"))
            {
                gameObjectToDoorInterface?.goThroughDoor();
            }

            if (Input.GetButton("Run") && !isInAir && !isCrouch && !dashDisable)
            {
                Speed = Run;
                PR.anim.SetTrigger("Running");
                PR.anim.ResetTrigger("Walking");

            }
            else if (dashDisable && Input.GetAxisRaw("Horizontal") != 0)
            {
                PR.anim.SetTrigger("Walking");
                Speed = 9f;
            }

            else if (Input.GetAxisRaw("Vertical") < 0f && !isInAir && isCrouch)
            {
                Speed = Crawl;

                PR.anim.SetBool("Crouch", false);
                PR.anim.ResetTrigger("Walking");
                PR.anim.ResetTrigger("Running");
            }
            else if (!Input.GetButton("Run") && !isInAir && !isCrouch)
            {
                Speed = Walk;
                PR.anim.ResetTrigger("Running");
            }
            else
            {
                PR.anim.ResetTrigger("Walking");
                PR.anim.ResetTrigger("Running");

                PR.anim.SetBool("Crouch", false);
                if (Escelator == null && !isInAir)
                    PR.anim.SetTrigger("Idle");

            }

            DetermineMovementStates();

            if (PR.RB.velocity.y < terminalVelocityY)
            {
                PR.RB.velocity = new Vector2(PR.RB.velocity.x, terminalVelocityY);
            }
        }

        else
        {
            PR.RB.velocity = new Vector2(0, 0);
            PR.anim.ResetTrigger("Walking");
            PR.anim.ResetTrigger("Running");

            PR.anim.SetBool("Crouch", false);
            PR.anim.SetTrigger("Idle");
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


            if (dirX == 0 && isInAir && PR.RB.velocity.x == 0 || !isInAir)
            {
                xForce = 0;
            }

            else if (dirX == 0 && isInAir && PR.RB.velocity.x != 0)
            {
                xForce = (PR.RB.velocity.x < 0 ? -Speed : Speed) / 2;
            }
        }
        else if(!canApplyAirMovement && dirX!=0)
        {
            canApplyAirMovement = !canApplyAirMovement;
        }

        if (!PR.Attack.bypassMoveBlock)
        {
            PR.RB.velocity = new Vector2(isInLandingLag || (PR.Attack.isAttacking && !isInAir) || isCrouch || PR.Attack.isSpecial || PR.anim.GetBool("hasGrabbedEnemy") ? 0 : (dirX > 0 ? dirX * smoothVector.x : -dirX * smoothVector.x)+ (xForce), PR.RB.velocity.y);
        }

        else
        {
           // Speed = Crawl;
            PR.RB.velocity = new Vector2((dirX > 0 ? dirX * smoothVector.x : -dirX * smoothVector.x), PR.RB.velocity.y);
        }

        if(PR.RB.velocity.x > Speed)
        {
            PR.RB.velocity = new Vector2(Speed, PR.RB.velocity.y);
        }
        else if(PR.RB.velocity.x < -Speed)
        {
            PR.RB.velocity = new Vector2(-Speed, PR.RB.velocity.y);
        }

        if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") >= 0f)
        {
            PR.anim.ResetTrigger("Walking");
            PR.anim.ResetTrigger("Running");
            
            PR.anim.SetBool("Crouch", false);
            PR.anim.SetTrigger("Idle");
            isCrouch = false;
        }

        else if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") < 0f && PR.anim.GetBool("Climbing") == false)
        {
            PR.RB.gravityScale = 1;
            PR.anim.ResetTrigger("Walking");
            PR.anim.ResetTrigger("Running");
            PR.anim.ResetTrigger("Idle");
            PR.anim.SetBool("Crouch", true);
            isCrouch = true;
        }

        else
        {
            if (dirX != 0 && !PR.Attack.isAttacking && !isInAir && !PR.anim.GetBool("hasGrabbedEnemy"))
                transform.eulerAngles = new Vector2(0, dirX < 0 ? 180 : 0);

            isLeft = transform.eulerAngles.y == 0 ? false : true;

            if (!isInAir && !(PR.anim.GetBool("Climbing") && !PR.anim.GetBool("isGrounded")))
            {
                switch (Speed)
                {
                    case 14f:
                        //isCrouch = false;
                        PR.anim.ResetTrigger("Walking");
                        break;

                    case 7f:
                        // isCrouch = false;
                        PR.anim.SetTrigger("Walking");
                        break;
                    case 3.5f:
                        //isCrouch = true;
                        PR.anim.ResetTrigger("Walking");
                        break;


                }
               
                PR.anim.SetBool("Crouch", false);
                PR.anim.ResetTrigger("Idle");
            }
        }

    }

    void Jump()
    {
        if (!(jumpAmt == 0 && isInAir && PR.anim.GetFloat("ClimbSpeed") == -1) && !isInLandingLag && !PR.Attack.isAttacking && !PR.anim.GetBool("hasGrabbedEnemy"))
        {
            if (PR.anim.GetBool("Climbing"))
                jumpAmt = 0;

            if (Input.GetButtonDown("Jump") && !PR.anim.GetAnimatorTransitionInfo(0).IsName("Idle -> Crouch"))
            {
                PR.anim.ResetTrigger("Idle");
                PR.anim.ResetTrigger("Climbing");
                isInAir = true;

                PR.anim.SetBool("isGrounded", !isInAir);
                if (jumpAmt < 2)
                {
                    jumpAmt++;
                    switch (jumpAmt)
                    {
                        case 1:
                            if (isInLandingLag == false)
                                PR.anim.SetBool("isJumping", true);
                            PR.anim.SetBool("isDoubleJumping", false);
                            break;
                        case 2:
                            PR.anim.SetBool("isJumping", false);
                            PR.anim.SetBool("isDoubleJumping", true);
                            PR.RB.gravityScale = 1.0f;
                            break;
                    }

                    if (PR.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Crouch")
                    {
                        PR.RB.velocity = new Vector2(PR.RB.velocity.x, jumpForce);
                        PR.anim.SetBool("isFalling", false);
                    }
                    else
                    {
                        print(PR.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                        StartCoroutine(SquatJump());
                    }

                }
            } 




            else if (Input.GetButtonUp("Jump"))
            {
                PR.RB.velocity = new Vector2(PR.RB.velocity.x, isFalling ? PR.RB.velocity.y : PR.RB.velocity.y / 2);
            }
        }



        if (PR.RB.velocity.y < 0.0f && Escelator == null)
        {
            if (!InEscelator)
                isFalling = true;
            //Debug.Log(PR.RB.velocity.y + "  " + isFalling);

            PR.anim.SetBool("isJumping", false);
            PR.anim.SetBool("isDoubleJumping", false);
            PR.anim.SetBool("isGrounded", !isFalling);
            
            PR.anim.SetBool("Crouch", false);
            if (!PR.Attack.SideBS)
                PR.RB.gravityScale = scaledGravity;
        }
        else isFalling = false;


        PR.anim.SetBool("isFalling", isFalling);


        //Crouch bug somewhere here
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
        if (!PR.anim.GetBool("Stunned") && !PR.anim.GetBool("Taunt") && !PR.anim.GetBool("Grabbing") && !PR.Stun.isAirSpin && !PR.anim.GetBool("isAirStunned") && !PR.anim.GetBool("isLaying") && !H.dead && !SS.ShieldStun && !isInGetup)
        {

            if (isInAir)
                Speed = Walk;
            if (!PR.anim.GetBool("EndLag"))
            {

                if (!PR.anim.GetBool("Climbing") && !PR.anim.GetBool("isAirStunned") && !PR.anim.GetBool("isLaying"))
                    Move();

                if (!PR.Attack.isExecutedOnce)
                    Jump();
            }
            PR.anim.SetBool("isGrounded", !isInAir);

            if (PR.RB.velocity.y < 0)
            {
                PR.anim.SetBool("isFalling", isFalling);
            }
            else PR.anim.SetBool("isFalling", isInAir);
        }

        else if (!PR.anim.GetBool("Stunned") && PR.Stun.isAirSpin)
        {
            if (globalDirX != 0 && PR.Stun.isAirSpin)
            {
                PR.Stun.isAirSpin = false;
                PR.anim.SetBool("isAirStunned", PR.Stun.isAirSpin);

            }
        }

        else if (!PR.anim.GetBool("Stunned") && PR.anim.GetBool("isLaying"))
        {
            if (globalDirX != 0)
            {
                PR.anim.SetBool("isLaying", false);
            }

            else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButton("Fire3"))
            {
                PR.anim.SetBool("canLayAttack", false);
            }
        }


        else if (PR.anim.GetBool("Stunned"))
        {
            PR.anim.ResetTrigger("Walking");
            PR.anim.ResetTrigger("Running");
            
            PR.anim.SetBool("Crouch", false);
            if (PR.RB.velocity.y < 0.0f)
            {
                isFalling = true;
                PR.anim.SetBool("isJumping", false);
                PR.anim.SetBool("isDoubleJumping", false);
                PR.anim.SetBool("isGrounded", !isInAir);
                PR.RB.gravityScale = scaledGravity;
            }
            else isFalling = false;
            PR.anim.SetBool("isJumping", false);
            PR.anim.SetBool("isFalling", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Grab") && collision.contacts[0].normal.y > .8f)
        {
            if(collision.gameObject.GetComponent<PlatformMovement>())
            {
                OnMoveable = true;
                transform.SetParent(collision.gameObject.transform);
                PR.RB.interpolation = RigidbodyInterpolation2D.None;
            }




            PR.RB.gravityScale /= PR.RB.gravityScale == scaledGravity ? scaledGravity : 1.0f;
            
            if (collision.gameObject.GetComponent<Escelator>())
                Escelator = collision.gameObject.GetComponent<Escelator>();
            if (PR.anim.GetBool("Climbing"))
                PR.anim.ResetTrigger("Climbing");
            if (PR.RB.velocity.y == 0 || Escelator != null )
                isInAir = false;


            if (!PR.anim.GetBool("Grabbing") && !isInAir)
            {
                jumpAmt = 0;
               PR.Attack.isExecutedOnce = false;
                isOnPassThrough = false;
            }



            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[0].transform.position, -Vector2.up * rayRange);
            Debug.DrawRay(groundRays[0].position, -Vector2.up * rayRange);

            if (collision.gameObject.GetComponent<PlatformEffector2D>() !=null)
            {
                print("On drop");
                currentPassThroughPlatform = collision.gameObject;
                isOnPassThrough = true;
                PR.anim.SetBool("isGrounded", isOnPassThrough);
                
                if(!PR.anim.GetCurrentAnimatorStateInfo(0).IsName("USpecial"))
                Physics2D.gravity = new Vector2(0, -9.81f);

                if (!PR.Attack.isAttacking && !isInAir)
                {
                    PR.anim.SetBool("isFalling", false);
                    jumpAmt = 0;
                   PR.Attack.isExecutedOnce = false;

                }


             /*   if (hitGround.collider.tag == "PassThroughPlatform" && !isInAir )
                {
                    jumpAmt = 0;
                }
                else jumpAmt = 0;
            */
            }


            if(PR.Stun.isAirSpin)
            {
                PR.anim.SetBool("Stunned", false);
                PR.anim.SetBool("isFalling", false);
                PR.anim.SetBool("isGrounded", true);
            }
            else PR.anim.SetBool("isGrounded", true);

        }

        if (PR.Attack.isInHelpless && PR.RB.velocity.y == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            PR.Attack.isInHelpless = !PR.Attack.isInHelpless;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if(OnMoveable)
        {
            OnMoveable = false;
            transform.SetParent(null);
            PR.RB.interpolation = RigidbodyInterpolation2D.Interpolate;
        }


        if (collision.gameObject.tag == "Platform")
        {
            if (!collision.gameObject.GetComponent<Escelator>())
            {
                Escelator = null;
                OnEscelator = false;

                if (Input.GetButton("Jump"))
                {
                    
                    PR.anim.SetBool("Crouch", false);
                    PR.anim.ResetTrigger("Idle");
                    isInAir = true;
                    PR.anim.SetBool("isGrounded", !isInAir);
                    PR.anim.SetBool("isJumping", true);
                    PR.anim.SetBool("isDoubleJumping", false);
                    PR.anim.SetBool("isFalling", false);
                }
            }

            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[1].transform.position, -Vector2.up * rayRange);
            if (!isInAir && 
                collision?.gameObject?.tag == "Platform" && 
                hitGround.collider?.tag == "Platform")
            {
                if (collision.gameObject.transform.position.y < transform.position.y)
                    jumpAmt = 1;
            }
            currentPassThroughPlatform = null;

            if (PR.anim.GetBool("Climbing"))
                isInAir = true;

            if (PR.RB.velocity.y != 0)
                isInAir = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "CutsceneTrigger")
        {
            collision.gameObject.GetComponent<CutsceneTrigger>().isTriggered = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Escelator>() && PR.RB.velocity.y < 0f)
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
        if (!PR.Attack.isAttacking && isOnPassThrough)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCol);
            yield return new WaitForSeconds(.5f);
            Physics2D.IgnoreCollision(playerCollider, platformCol, false);
        }
        else isOnPassThrough = true;
        isCoroutineRunning = false;

    }

    private IEnumerator SquatJump()
    {
            yield return new WaitForSeconds(.75f);
         PR.RB.velocity = new Vector2(PR.RB.velocity.x, 10f);
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
        return PR.anim.GetBool("Stunned");
    }

    public void SetIsOnPassThrough(bool newPT)
    {
        isOnPassThrough = newPT;
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