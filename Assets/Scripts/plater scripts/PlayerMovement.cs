using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    private Collider2D playerCollider;
    private Animator anim;
    public Transform cam;
    public Vector3 offset;

    public float Walk, jumpForce, fallingGravityFactor;
    float Speed, Run, Crawl, scaledGravity, jumpAmt;

    Material WalkMat;
    public Material crouchMat, RunMat;

    private GameObject currentPassThroughPlatform;

    public bool isFalling = false;
    private bool isLeft = false;
    public bool isInAir = false;
    public bool isInLandingLag = false;
    private bool isOnPassThrough = false;
    public bool OnLadder = false;
    bool isCrouch = false;

    public Transform[] groundRays;
    public float rayRange = 5f;

    public PlayerAttack attackScript;

    private Stun S;
    private bool stunned;

    private Escelator Escelator;
    public bool OnEscelator , InEscelator = false;

    private float lastY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        attackScript = GetComponent<PlayerAttack>();
        WalkMat = sr.material;
        Speed = Walk;
        Run = Walk * 2;
        Crawl = Walk / 2;
        scaledGravity = rb.gravityScale * fallingGravityFactor;
        jumpAmt = 0;

        S =GetComponent<Stun>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        stunned = S.getIsStunned();

        if(Escelator != null)
        OnEscelator = Escelator.GetOnEscelator();

        isInLandingLag = anim.GetCurrentAnimatorStateInfo(0).IsName("Fall 2 Idle");
        cam.position = this.transform.position + offset;

        isCrouch = anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch");

        if (Input.GetKey(KeyCode.LeftShift) && !isInAir && !isCrouch)
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
        else if (!Input.GetKey(KeyCode.LeftShift) && !isInAir && !isCrouch)
        {
            Speed = Walk;
            anim.ResetTrigger("Running");
        }
        else
         {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            if(Escelator == null)
            anim.SetTrigger("Idle");
         }

        if(!stunned)
        {
        Move();
        Jump();
        }
        else
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
    }

    void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(isInLandingLag || (attackScript.isAttacking && !isInAir) || isCrouch ? 0 : dirX * Speed, rb.velocity.y);

        if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") >= 0f)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Crouch");
            anim.SetTrigger("Idle");
            //isCrouch = false;
        }

        else if (dirX == 0 && !isInAir && Input.GetAxisRaw("Vertical") < 0f && !isOnPassThrough && !OnLadder)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Crouch");
            //isCrouch = true;
        }

        else
        {
            if (dirX != 0)
                transform.eulerAngles = new Vector2(0, dirX < 0 ? 180 : 0);

            isLeft = transform.eulerAngles.y == 0 ? false : true;

            if (!isInAir && !OnLadder)
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
                        //  isCrouch = true;
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
        if (!(jumpAmt == 0 && isInAir) && !isInLandingLag && !attackScript.isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.ResetTrigger("Crouch");
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
                            anim.SetBool("isJumping", true);
                            anim.SetBool("isDoubleJumping", false);
                            break;
                        case 2:
                            anim.SetBool("isJumping", false);
                            anim.SetBool("isDoubleJumping", true);
                            break;
                    }
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                    if(OnLadder && jumpAmt == 2)
                    jumpAmt = 1;
                }
            }



            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, isFalling ? rb.velocity.y : 0.0f);
            }
        }


        if (rb.velocity.y < 0.0f && Escelator == null)
        {
            if (!InEscelator)
            isFalling = true;
           

            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isGrounded", !isInAir);
            anim.ResetTrigger("Crouch");
            rb.gravityScale = scaledGravity;
        }
        else isFalling = false;


        anim.SetBool("isFalling", isFalling);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPassThroughPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform")
        {
            if (collision.gameObject.GetComponent<Escelator>())
                Escelator =collision.gameObject.GetComponent<Escelator>();


            rb.gravityScale /= rb.gravityScale == scaledGravity ? scaledGravity : 1.0f;

            if (rb.velocity.y == 0 || Escelator != null)
                isInAir = false;

            anim.SetBool("isGrounded", !isInAir);

            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[0].transform.position, -Vector2.up * rayRange);

            if (collision.gameObject.tag == "PassThroughPlatform")
            {
                isOnPassThrough = true;
                if (hitGround.collider.tag == "PassThroughPlatform" && !isInAir)
                {
                    jumpAmt = 0;
                }
                currentPassThroughPlatform = collision.gameObject;
            }
            if (collision != null && collision.gameObject.tag == "Platform" && hitGround.collider.tag == "Platform")
            {
                jumpAmt = 0;
                isOnPassThrough = false;
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

                if (Input.GetKey(KeyCode.Space))
                {
                anim.ResetTrigger("Crouch");
                anim.ResetTrigger("Idle");
                isInAir = true;
                anim.SetBool("isGrounded", !isInAir);
                anim.SetBool("isJumping", true);
                anim.SetBool("isDoubleJumping", false);
                jumpAmt =1 ;
                }
            }

            RaycastHit2D hitGround = Physics2D.Raycast(groundRays[1].transform.position, -Vector2.up * rayRange);
            if (!isInAir && collision.gameObject.tag == "Platform" && hitGround.collider.tag == "Platform" || collision.gameObject.tag == "PassThroughPlatform" && hitGround.collider.tag == "PassThroughPlatform")
            {
                jumpAmt = 1;
            }
            currentPassThroughPlatform = null;

            if(OnLadder)
            isInAir = OnLadder;
        }

       
        if (rb.velocity.y != 0 )
        isInAir = true;
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
              OnLadder = true;
              anim.SetBool("OnLadder", OnLadder);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Escelator>() && rb.velocity.y < 0f)
        {
            InEscelator=true;
        }

        if(OnLadder && rb.gravityScale == 0)
        {
          anim.SetTrigger("Climbing");
          
          float CurrnetY = transform.position.y;          
          if(CurrnetY == lastY)
          {
            anim.SetFloat("ClimbSpeed",0f);
            
          }
          if(CurrnetY < lastY)
          {
            anim.SetFloat("ClimbSpeed",-1f);
            
          }
          if(CurrnetY > lastY)
          {
            anim.SetFloat("ClimbSpeed",1f);
          }
          lastY = CurrnetY; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Ladder")
       {
              OnLadder = false;
              anim.ResetTrigger("Climbing");
              anim.SetBool("OnLadder", OnLadder);
       }

       if (collision.gameObject.GetComponent<Escelator>())
            {
            InEscelator=false;
            }
    }


    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCol = currentPassThroughPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCol);
        yield return new WaitForSeconds(.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCol, false);
    }

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
}