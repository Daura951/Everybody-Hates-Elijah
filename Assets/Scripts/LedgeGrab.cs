using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    public bool reGrab = true, action = false, grab = true, isLeft;
    Collider2D greenBox, redBox;
    GameObject g;
    private float greenX, redX;
    private Rigidbody2D rb;

    [Header("ledge colliders")]
    public float redXOff;
    public float redYOff, redXSize, redYSize, greenXOff, greenYOff, greenXSize, greenYSize;


    public LayerMask groundMask;

    [Header("Timers")]
    public float holding, Limit;
    private float timer, timer1;

    PlayerMovement pm;
    PlayerAttack PA;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        PA = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        greenX = greenXOff;
        redX = redXOff;
    }

    // Update is called once per frame
    void Update()
    {
        isLeft = pm.GetIsLeft();
        anim.SetBool("Grabbing", pm.grabbing);

        if (!pm.grabbing)
        {
            Grab();
            action = false;
        }

        if (pm.grabbing && !action)
        {
            if (timer1 >= holding)
            {
                anim.Play("Ledge idle");
                timer1 = holding;
            }
            if (timer1 == holding)
            {
                timer1 = 0;
            }
            else timer1 += Time.deltaTime;


            if (timer >= Limit)
            {
                timer = Limit;
            }
            if (timer == Limit)
            {
                Reseting(-75f, 1f);
            }
            else timer += Time.deltaTime;

            // allows it so player does immedialty get off
            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                grab = true;
            }

            // fall down
            if (Input.GetAxisRaw("Vertical") < 0f)
            {
                Reseting(-75f, 1f);
            }


            //jump up
            if (Input.GetButtonDown("Jump"))
            {
                action = true;
                pm.grabbing = false;
                rb.gravityScale = 1f;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                action = true;
                anim.Play("Ledge attack");
            }




            if (transform.position.x < g.transform.position.x)
            {
                if (Input.GetAxisRaw("Horizontal") < 0f && grab)
                {
                    Reseting(-75f, 1f);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0f && grab)
                {
                    action = true;
                    anim.Play("Ledge pull");
                }
            }

            else
            {
                if (Input.GetAxisRaw("Horizontal") > 0f && grab)
                {
                    Reseting(-75f, 1f);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0f && grab)
                {
                    action = true;
                    anim.Play("Ledge pull");
                }
            }

        }

    }

    void Grab()
    {
        if (transform.eulerAngles.y == 0)
        {
            greenXOff = greenX;
            redXOff = redX;
        }
        else
        {
            greenXOff = -greenX;
            redXOff = -redX;
        }


        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOff * transform.localScale.x), transform.position.y + greenYOff), new Vector2(greenXSize, greenYSize), 0f, groundMask);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOff * transform.localScale.x), transform.position.y + redYOff), new Vector2(redXSize, redYSize), 0f, groundMask);


        if ((!greenBox && redBox && !pm.grabbing && !pm.GetIsStunned() && reGrab) || ((greenBox || redBox) && (PA.SideBS || PA.ASideB)))
        {

            if (redBox)
                g = redBox.gameObject;
            if (greenBox)
                g = greenBox.gameObject;


            if (g.CompareTag("Platform"))
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;


                if (transform.position.x < g.transform.position.x)
                {
                    transform.position = new Vector2(g.transform.position.x - (g.transform.localScale.x / 2) - .1f, g.transform.position.y);

                    if (transform.localEulerAngles.y != 0)
                        transform.eulerAngles = new Vector2(0, !pm.GetIsLeft() ? 180 : 0);
                }
                else
                {
                    transform.position = new Vector2(g.transform.position.x + (g.transform.localScale.x / 2) + .1f, g.transform.position.y);

                    if (transform.localEulerAngles.y == 0)
                        transform.eulerAngles = new Vector2(0, !pm.GetIsLeft() ? 180 : 0);
                }



                pm.grabbing = true;
                reGrab = false;
                timer1 = timer = 0;
                anim.SetBool("isJumping", false);
                anim.SetBool("isDoubleJumping", false);
                anim.SetBool("isFalling", false);
                anim.Play("Ledge grab");
                PA.isAttacking = PA.isSpecial = PA.isExecutedOnce = PA.SideBS = PA.ASideB = false;
                if (pm.jumpAmt == 2)
                    pm.jumpAmt = 1;

                if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
                {
                    grab = false;
                }


                for (int i = 0; i < PA.hitBoxes.Length; i++)
                {
                    PA.DespawnHitBox(i);
                }

            }
        }

        if ((!greenBox && !redBox) || anim.GetBool("isGrounded"))
        {
            reGrab = true;
        }

    }


    public void PullUp()
    {
        if (transform.position.x < g.transform.position.x)
        {
            transform.position = new Vector2(g.transform.position.x - (g.transform.localScale.x / 2) + .5f, g.transform.position.y + 1.1f);

        }
        else
        {
            transform.position = new Vector2(g.transform.position.x + (g.transform.localScale.x / 2) - .5f, g.transform.position.y + 1.1f);

        }
        pm.grabbing = false;
        rb.gravityScale = 1f;

    }

    public void Attack()
    {
        if (transform.position.x < g.transform.position.x)
        {
            transform.position = new Vector2(g.transform.position.x - (g.transform.localScale.x / 2) + .5f, g.transform.position.y + 1.1f);

        }
        else
        {
            transform.position = new Vector2(g.transform.position.x + (g.transform.localScale.x / 2) - .5f, g.transform.position.y + 1.1f);

        }
        pm.grabbing = false;
        rb.gravityScale = 1f;

    }

    private void Reseting(float a, float b)
    {
        action = true;
        GetOff(a, b);
        pm.grabbing = false;

        anim.SetBool("Grabbing", false);
        if (a < 0)
            anim.SetBool("isFalling", true);
        else
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isDoubleJumping", true);
        }
    }


    private void GetOff(float angle, float Kb)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * Kb;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * Kb;



        if (!isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent), ForceMode2D.Impulse);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOff * transform.localScale.x), transform.position.y + redYOff), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOff * transform.localScale.x), transform.position.y + greenYOff), new Vector2(greenXSize, greenYSize));
    }
}