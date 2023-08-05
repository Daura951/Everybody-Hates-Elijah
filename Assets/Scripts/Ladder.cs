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
    public bool climb , grounded;

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
        LadderCheck();
        if(climb)
        {
         if (Input.GetKey(KeyCode.W))
         {
           player.transform.position += new Vector3(0,1,0) * Time.deltaTime * speed;
         }
         if (Input.GetKey(KeyCode.S))
         {
           player.transform.position -= new Vector3(0,1,0) * Time.deltaTime * speed;
         }
        }
    }

    private void LadderCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = PGravity;
        } 
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(0,0);
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
     if(col.gameObject == player)
     {
       climb = true;
       rb.velocity = new Vector2(0,0);
     }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
     if(col.gameObject == player)
     {
      climb = false;
      rb.gravityScale = PGravity;
     }
    }
}