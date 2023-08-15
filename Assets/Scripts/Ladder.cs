using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    PlayerMovement PM;
    Rigidbody2D rb;
    private Animator anim;
    GameObject player;


    public float speed = 1;
    private float PGravity;
    public bool OnLadder , climb , grounded , Stunned;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody2D>();
        anim = player.GetComponent<Animator>();
        PGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
         grounded = !PM.GetIsInAir();
         Stunned = PM.GetIsStunned();

         if (OnLadder && !Stunned)
         {
            if ((Input.GetAxisRaw("Vertical") > 0 && !Input.GetButton("Jump")) || (Input.GetAxisRaw("Vertical")<0 && !grounded && !Input.GetButton("Jump")))
            {
                climb = true;
            }
            else if (grounded && !Input.GetKey(KeyCode.W))
            {
                climb = false;
            }
         }
         
         if(climb)
         {
          LadderCheck();
          
          if (Input.GetAxisRaw("Vertical") > 0)
          {
           anim.SetTrigger("Climbing");
           anim.SetFloat("ClimbSpeed",1f);
           anim.SetBool("isGrounded", grounded);
           player.transform.position += new Vector3(0,1,0) * Time.deltaTime * speed;
          }
          
          else if (Input.GetAxisRaw("Vertical") < 0 && !Input.GetButton("Jump"))
          {
           if(!grounded)
           {
             anim.SetTrigger("Climbing");
             anim.SetFloat("ClimbSpeed",-1f);
             player.transform.position -= new Vector3(0,1,0) * Time.deltaTime * speed;
           } 
           if(grounded)
            anim.ResetTrigger("Climbing"); 
          
          }

          else
          {
            anim.SetFloat("ClimbSpeed",0f);
            anim.ResetTrigger("Climbing"); 
          }

          if(anim.GetBool("Climbing"))
          {
            PM.transform.position = new Vector2(this.transform.position.x , PM.transform.position.y);
            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJumping", false);
          }


         }
    }

    private void LadderCheck()
    {
        if (Input.GetButton("Jump") || grounded)
        {
            rb.gravityScale = PGravity;
            climb = false;
        } 
        else if (Input.GetAxisRaw("Vertical") > 0 || (Input.GetAxisRaw("Vertical") < 0 && !grounded))
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
        } 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
     if(col.gameObject == player)
     {
       OnLadder = true;
       anim.SetBool("OnLadder", OnLadder);
     }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            OnLadder = false;
            climb = false;
            anim.SetBool("OnLadder", OnLadder);
            anim.ResetTrigger("Climbing");
            rb.gravityScale = PGravity;
        }
    }
}