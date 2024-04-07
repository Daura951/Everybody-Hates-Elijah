using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    private Vector3 home , End;
    public Vector3 EndSpot;
    public float speed;
    private bool Fall, bounce = true;
    PlayerMovement PM;
    Rigidbody2D rb;
    GameObject player;
    private BladeBound BB;

    private float startSpeed;
    private float halfSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        PM = player.GetComponent<PlayerMovement>();
        home = this.transform.position;
        End = this.transform.position + EndSpot;
        BB = player.GetComponent<BladeBound>();
        startSpeed = speed;
        halfSpeed = speed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Fall = PM.GetIsFalling();

        if(BB.GetIsInBladeBound() && !BB.isFrozen)
        {
            speed = halfSpeed;
        }
        else if(BB.isFrozen)
        {
            speed = 0;
        }
        else
        {
            speed = startSpeed;
        }
    }


    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;

        if (bounce)
        {
            transform.position = Vector2.MoveTowards(transform.position, End, step);
            if (transform.position == End)
                bounce = !bounce;
        }


        if (!bounce)
        {
            transform.position = Vector2.MoveTowards(transform.position, home, step);
            if (transform.position == home)
                bounce = !bounce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.tag == "Player")
            {
              player.transform.SetParent(transform);
              rb.interpolation = RigidbodyInterpolation2D.None; 
            }
    }

     private void OnCollisionExit2D(Collision2D collision)
     {
            if (collision.gameObject.tag == "Player" && collision.contacts[0].normal.y < -0.8f)
            {
              player.transform.SetParent(null);
              rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
     }
}