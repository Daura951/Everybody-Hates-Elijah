using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    private Vector3 home , End;
    public Vector3 EndSpot;
    public float speed;
    private bool Fall , bounce = true;
    PlayerMovement PM;
    GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        home = this.transform.position;
        End = this.transform.position + EndSpot;
    }

    // Update is called once per frame
    void Update()
    {
        Fall = PM.GetIsFalling();
        float step = speed * Time.deltaTime;
        
               if(bounce)
        {
        transform.position = Vector2.MoveTowards(transform.position, End, step);
            if(transform.position == End)
                bounce = !bounce;
        }


        if(!bounce)
        {
        transform.position = Vector2.MoveTowards(transform.position, home, step);
            if(transform.position == home)
                bounce = !bounce;
        }  
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
            if (collision.gameObject.tag == "Player" && Fall)
            {
                collision.collider.transform.SetParent(transform);
            }
     }

     private void OnCollisionExit2D(Collision2D collision)
     {
            if (collision.gameObject.tag == "Player")
            {
                collision.collider.transform.SetParent(null);
            }
     }
}