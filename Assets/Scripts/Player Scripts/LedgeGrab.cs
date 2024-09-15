using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    public bool reGrab = true, action = false, grab = false, moveable = false;
    Collider2D redBox;
    BoxCollider2D Box;
    GameObject g;
    private float redX, XoffSet;


    [Header("ledge colliders")]
    public float redXOff, redYOff, redXSize, redYSize;
    public LayerMask groundMask;

    [Header("Timers")]
    public float holding, Limit;
    private float timer, timer1;


    public PlayerRefrecnces PR;

    PlayerOffScreen POS;

    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        POS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerOffScreen>();

        redX = redXOff;
        XoffSet = transform.localScale.x * .375f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!PR.anim.GetBool("Grabbing") && Input.GetAxisRaw("Vertical") >= 0f)
        {
            Grab();
            action = false;
        }

        // timers for falling and inputs to leave
        if (PR.anim.GetBool("Grabbing") && !action)
        {
            Offset();
            transform.position = offset;



            if (moveable)
                transform.SetParent(g.transform);



            if (timer1 >= holding)
            {
                PR.anim.Play("Ledge idle");
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
                if (moveable)
                {
                    moveable = false;
                    transform.SetParent(null);
                }


                action = true;
                PR.anim.SetBool("Grabbing",false);
                PR.RB.gravityScale = 1f;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                action = true;
                PR.anim.Play("Ledge attack");
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
                    PR.anim.Play("Ledge pull");
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
                    PR.anim.Play("Ledge pull");
                }
            }

        } 

    }

    void Grab()
    {
        if (transform.eulerAngles.y == 0)
        {
            redXOff = redX;
        }
        else
        {
            redXOff = -redX;
        }
        
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOff * transform.localScale.x), transform.position.y + redYOff), new Vector2(redXSize, redYSize), 0f, groundMask);

        if (!redBox) 
            reGrab = true;
        
        if (!reGrab)
            grab = false;

        if (( redBox && !PR.anim.GetBool("Grabbing") && !PR.Move.GetIsStunned()) || ( redBox && (PR.Attack.SideBS || PR.Attack.ASideB)))
        {

            if (redBox)
            {
               g = redBox.gameObject;
               Box= g.GetComponent<BoxCollider2D>();
            }

            PR.Attack.bypassMoveBlock = false;

            if (g.GetComponent<PlatformMovement>())
                moveable = true;

            if (g.CompareTag("Grab") && reGrab)
            {
                PR.RB.velocity = Vector2.zero;
                PR.RB.gravityScale = 0;
                reGrab = false;
               PR.anim.SetBool("Grabbing", true);
                timer1 = timer = 0;
                PR.anim.SetBool("isJumping", false);
                PR.anim.SetBool("isDoubleJumping", false);
                PR.anim.SetBool("isFalling", false);
                PR.anim.Play("Ledge grab");
                PR.Move.isFalling = PR.Move.isInAir = PR.Attack.isAttacking = PR.Attack.isSpecial = PR.Attack.isExecutedOnce = PR.Attack.SideBS = PR.Attack.ASideB = false;
                if (PR.Move.jumpAmt == 2)
                    PR.Move.jumpAmt = 1;

                if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
                {
                    grab = false;
                }


                for (int i = 0; i < PR.Attack.hitBoxes.Length; i++)
                {
                    PR.Attack.DespawnHitBox(i);
                }

            }    
        }
    }


    public void Offset()
    {
        if (transform.position.x < g.transform.position.x)
        {
            offset = new Vector2((g.transform.position.x - (g.transform.localScale.x * 0.5f) - XoffSet + (Box.offset.x * Box.size.x)), (g.transform.position.y - (((1f - g.transform.localScale.y) / .25f) * .125f) + (Box.offset.y * Box.size.y)));
            if (transform.localEulerAngles.y == 0)
            {
                gameObject.transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
        else
        {
            offset = new Vector2((g.transform.position.x + (g.transform.localScale.x * 0.5f) + XoffSet - (Box.offset.x * Box.size.x)), (g.transform.position.y - (((1f - g.transform.localScale.y) / .25f) * .125f) + (Box.offset.y * Box.size.y)));
            if (transform.localEulerAngles.y != 0)
            {
                gameObject.transform.eulerAngles = new Vector3(0,0,0);
            }
        }
    }


    public void PullUp()
    {   
        if (transform.position.x < g.transform.position.x)
        {
            transform.position = new Vector2(transform.position.x +1.575f, 0.43f * g.transform.localScale.y + g.transform.position.y + 0.55f + Box.offset.y * Box.size.y);
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - 1.575f, 0.43f * g.transform.localScale.y + g.transform.position.y + 0.55f + Box.offset.y * Box.size.y);
            gameObject.transform.eulerAngles = new Vector3(0, -180, 0);
        }
        PR.anim.SetBool("Grabbing",false);
        PR.RB.gravityScale = 1f;

    }



    public void Attack()
    {
        if (transform.position.x < g.transform.position.x)
        {
            transform.position = new Vector2(transform.position.x + 1.5f, (transform.position.y + 1.02f));
        }
        else
        {
            transform.position = new Vector2(transform.position.x - 1.5f, (transform.position.y + 1.02f));

        }
        PR.anim.SetBool("Grabbing",false);
        PR.RB.gravityScale = 1f;

    }

    public void CamUpdate(string XY)
    {
        string[] Info = XY.Split(" ");
        float x = float.Parse(Info[0]);
        float y = float.Parse(Info[1]);
        if (transform.position.x < g.transform.position.x)
        {
            POS.OffsetCam( new Vector3(x, y, -0.75f));
        }
        else
        {
            POS.OffsetCam(new Vector3(-x, y, -0.75f));

        }
    }

    private void Reseting(float a, float b)
    {
        if (Input.GetAxisRaw("Vertical") >= 0)
            action = true;
        GetOff(a, b);
        PR.anim.SetBool("Grabbing",false);
        moveable = false;
            transform.SetParent(null);
        PR.anim.SetBool("Grabbing", false);
        if (a < 0)
            PR.anim.SetBool("isFalling", true);
        else
        {
            PR.anim.SetBool("isFalling", false);
            PR.anim.SetBool("isDoubleJumping", true);
        }
    }


    private void GetOff(float angle, float Kb)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * Kb;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * Kb;



        if (!PR.Move.isLeft)
        {
            XComponent *= -1;
        }

        PR.RB.AddForce(new Vector2(XComponent, YComponent), ForceMode2D.Impulse);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOff * transform.localScale.x), transform.position.y + redYOff), new Vector2(redXSize, redYSize));
    }
 
}