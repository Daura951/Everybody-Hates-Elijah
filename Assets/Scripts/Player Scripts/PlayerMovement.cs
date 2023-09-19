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
    private Animator anim;
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
    public float terminalVelocityY =  -10f;

    public Transform[] groundRays;
    public float rayRange = 5f;

    [Header("Attacks")]
    public PlayerAttack attackScript;

    private Stun S;
    private bool stunned;

    [Header("Escelator")]
    private Escelator Escelator;
    public bool OnEscelator, InEscelator = false;



    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        ChangeHealth();

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        attackScript = GetComponent<PlayerAttack>();
        Speed = Walk;
        Run = Walk * 2;
        Crawl = Walk / 2;
        scaledGravity = rb.gravityScale * fallingGravityFactor;
        jumpAmt = 0;


        S = GetComponent<Stun>();

        if(terminalVelocityY > 0)
        {
            terminalVelocityY *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpSqaut"))
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        stunned = S.getIsStunned();

        if (Escelator != null)
            OnEscelator = Escelator.GetOnEscelator();

        isInLandingLag = anim.GetCurrentAnimatorStateInfo(0).IsName("Fall 2 Idle");
        cam.position = this.transform.position + offset;

        isCrouch = anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch");

        if (Input.GetButton("Run") && !isInAir && !isCrouch)
        {
            Speed = Run;
            anim.SetTrigger("Running");
            anim.ResetTrigger("Walking");

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

        if (!stunned && !anim.GetBool("Taunt"))
        {
            if(!anim.GetBool("Climbing"))
            Move();

            if(!PlayerAttack.attackInstance.isExecutedOnce)
            Jump();
        }
        else if (stunned)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            anim.SetTrigger("Idle");
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
            anim.SetBool("isFalling", isFalling);
        }

        if(rb.velocity.y < terminalVelocityY)
        {
            rb.velocity = new Vector2(rb.velocity.x, terminalVelocityY);
        }
    }

    void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(isInLandingLag || (attackScript.isAttacking && !isInAir) || isCrouch || attackScript.isSpecial ? 0 : dirX * Speed, rb.velocity.y);

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
            if (dirX != 0 && !attackScript.isAttacking && !isInAir)
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

                    case 9f:
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
        if (!(jumpAmt == 0 && isInAir && !anim.GetBool("Climbing")) && !isInLandingLag && !attackScript.isAttacking)
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
                            if(isInLandingLag==false)
                                anim.SetBool("isJumping", true);
                                anim.SetBool("isDoubleJumping", false);
                            break;
                        case 2:
                            anim.SetBool("isJumping", false);
                            anim.SetBool("isDoubleJumping", true);
                            rb.gravityScale = 1.0f;
                            break;
                    }

                    if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name !="Crouch")
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                }
            }



            else if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, isFalling ? rb.velocity.y : rb.velocity.y/2);
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
            if(!PlayerAttack.attackInstance.SideBS)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform")
        {

            Physics2D.gravity = new Vector2(0 , -9.81f);

            if (collision.gameObject.GetComponent<Escelator>())
                Escelator = collision.gameObject.GetComponent<Escelator>();

            if (anim.GetBool("Climbing"))
                anim.ResetTrigger("Climbing");


            rb.gravityScale /= rb.gravityScale == scaledGravity ? scaledGravity : 1.0f;

            if (rb.velocity.y == 0 || Escelator != null)
                isInAir = false;



            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[0].transform.position, -Vector2.up * rayRange);
            Debug.DrawRay(groundRays[0].position, -Vector2.up * rayRange);

            if (collision.gameObject.tag == "PassThroughPlatform")
            {
                print("Passthrough");
                isOnPassThrough = true;
                
                if (hitGround.collider.tag == "PassThroughPlatform" && !isInAir)
                {
                    jumpAmt = 0;
                }
                currentPassThroughPlatform = collision.gameObject;
            }
            else if (collision != null && collision.gameObject.tag == "Platform" && hitGround.collider.tag == "Platform")
            {
                jumpAmt = 0;
                PlayerAttack.attackInstance.isExecutedOnce = false;
                isOnPassThrough = false;
                anim.SetBool("isGrounded", !isInAir);
            }

            if (collision.gameObject.transform.position.y+.5f < transform.position.y)
            {
                jumpAmt = 0;
                PlayerAttack.attackInstance.isExecutedOnce = false;
                anim.SetBool("isGrounded", true);
            }
            
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PassThroughPlatform" || collision.gameObject.tag == "Platform")
        {
            if (collision.gameObject.GetComponent<Escelator>())
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
                    jumpAmt = 1;
                }
            }

            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[1].transform.position, -Vector2.up * rayRange);
            if (!isInAir && collision.gameObject.tag == "Platform" && hitGround.collider.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform" && hitGround.collider.tag == "PassThroughPlatform")
            {
                if(collision.gameObject.transform.position.y < transform.position.y)
                jumpAmt = 1;
            }
            currentPassThroughPlatform = null;

            if (anim.GetBool("Climbing"))
                isInAir = true;

            if (rb.velocity.y != 0)
                isInAir = true;
        }
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
       if(collision.gameObject.tag == "Ladder")
       {
              Speed = Walk;
              if(isInAir)
              jumpAmt=1;
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
        healthText.text = ""+GetComponent<Health>().health;
    }
}