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
    float Speed,Run, Crawl, scaledGravity, jumpAmt;
  
    Material WalkMat;
    public Material crouchMat , RunMat;

    private GameObject currentPassThroughPlatform;

    private bool isFalling = false;
    private bool isLeft = false;

    

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       playerCollider = GetComponent<Collider2D>();
       anim = GetComponent<Animator>();
       WalkMat =sr.material;
       Speed = Walk;
       Run = Walk * 2;
       Crawl = Walk/2;
       scaledGravity = rb.gravityScale * fallingGravityFactor;
       jumpAmt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = this.transform.position + offset;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = Run;
            anim.SetTrigger("Running");
            anim.ResetTrigger("Walking");

        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Speed = Crawl;
            sr.material = crouchMat;
        }
        else
        {
            Speed = Walk;
            anim.ResetTrigger("Running");
        }

        Move();
        Jump();
    }

    void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * Speed, rb.velocity.y);

        if (dirX == 0)
        {
            anim.ResetTrigger("Walking");
            anim.ResetTrigger("Running");
            anim.SetTrigger("Idle");
        }
        else
        {

           this.transform.eulerAngles = new Vector2(0, dirX < 0 ? 180 : 0);
           isLeft = dirX > 0 ? false : true;

            switch (Speed)
            {
                case 14f:
                    anim.ResetTrigger("Walking");
                    break;

                case 7f:
                    anim.SetTrigger("Walking");
                    break;

            }

            anim.ResetTrigger("Idle");
        }
        
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpAmt < 2)
            {
                jumpAmt++;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
                
        }


        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, isFalling ? rb.velocity.y : 0.0f);
        }


        if (rb.velocity.y < 0.0f)
        {
            isFalling = true;
            rb.gravityScale = scaledGravity;
        }
        else isFalling = false;

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
            rb.gravityScale /= rb.gravityScale == scaledGravity ? scaledGravity: 1.0f;


            if (collision.gameObject.tag == "PassThroughPlatform")
            {
                currentPassThroughPlatform = collision.gameObject;
            }

        }

        jumpAmt = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PassThroughPlatform" || collision.gameObject.tag == "Platform")
        {
            jumpAmt = 1;
            currentPassThroughPlatform = null;
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
}
