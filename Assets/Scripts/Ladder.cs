using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    PlayerMovement PM;
    Rigidbody2D rb;
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
           player.transform.position += new Vector3(0,1,0) * Time.deltaTime * speed;
          }
          else if (Input.GetAxisRaw("Vertical") < 0 && !Input.GetButton("Jump") && !grounded)
          {
           player.transform.position -= new Vector3(0,1,0) * Time.deltaTime * speed;
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
     }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            OnLadder = false;
            climb = false;
            rb.gravityScale = PGravity;
        }
    }
}