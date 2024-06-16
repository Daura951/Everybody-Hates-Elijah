using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    public PlayerRefrecnces PR;
    public float speed;
    
    Collider2D TopBox;
    private float gravity;
    private bool attacking;

    // Start is called before the first frame update
    private void Start()
    {
        gravity = PR.RB.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        attacking = PR.Attack.GetAttacking();
        CheckOnLadder();
        if(PR.anim.GetBool("OnLadder"))
        {
        CheckClimb();
        Climb();
        }
    }

    private void CheckOnLadder()
    {
        TopBox = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - .1f), new Vector2(.25f, 1.2f), 0f, 1);
        if (TopBox)
        {
            if (TopBox.gameObject.CompareTag("Ladder"))
                PR.anim.SetBool("OnLadder", true);
        }
        else
        {
            if (PR.anim.GetAnimatorTransitionInfo(0).IsName("Climbing -> Single Jump Fall") && PR.RB.gravityScale == 0)
                PR.RB.gravityScale = gravity;
            PR.anim.SetBool("OnLadder", false);
            PR.anim.SetBool("Climbing", false);
        }
    }

    private void CheckClimb()
    {
        if(!PR.anim.GetBool("Stunned") && !attacking)
        {
            if (Input.GetAxisRaw("Vertical") != 0 && !PR.anim.GetBool("isGrounded") && !Input.GetButton("Jump"))
            {
                PR.RB.velocity = new Vector2(0, 0);
                PR.RB.gravityScale = 0;
                PR.anim.SetBool("Climbing",true);
                PR.Attack.isExecutedOnce = false;
                transform.position = new Vector2(transform.position.x, transform.position.y);
                PR.anim.SetBool("isJumping", false);
                PR.anim.SetBool("isDoubleJumping", false);
            }
            else if ((PR.anim.GetBool("isGrounded") && Input.GetAxisRaw("Vertical") < 0) || Input.GetButton("Jump"))
            {
                PR.RB.gravityScale = gravity;
                PR.anim.SetBool("Climbing",false);
            }
        }
    }

    private void Climb()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && !Input.GetButton("Jump"))
        {
            PR.anim.SetFloat("ClimbSpeed", 1f);
            PR.anim.SetBool("isGrounded", false);
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * speed;
        }

        else if (Input.GetAxisRaw("Vertical") < 0 && !Input.GetButton("Jump"))
        {
            if (!PR.anim.GetBool("isGrounded"))
            {
                PR.anim.SetTrigger("Climbing");
                PR.anim.SetFloat("ClimbSpeed", -1f);
                transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * speed;
            }

        }

        else
        {
            PR.anim.SetFloat("ClimbSpeed", 0f);
            PR.anim.SetBool("Climbing", false);
        }
    }
}
