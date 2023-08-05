using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public Vector3 LowStart;
    public Vector3 HighEnd;
    public float speed;
    private float Xspeed , Yspeed;
    private bool Fall , bounce;
    PlayerMovement PM;
    [SerializeField] GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        PM = player.GetComponent<PlayerMovement>();
        Xspeed = Yspeed = speed;
        bounce = true;
    }

    // Update is called once per frame
    void Update()
    {
        Fall = PM.GetIsFalling();
        
               if (LowStart.x != HighEnd.x){

                transform.position +=  new Vector3(1f , 0 ,0) * Xspeed * Time.deltaTime;

                if(transform.position.x >= HighEnd.x && bounce)
                {
                    Xspeed *= -1;
                    bounce = false;
                }
                if(transform.position.x <= LowStart.x && !bounce)
                {
                    Xspeed *= -1;
                    bounce = true;
                }
               }

               if(LowStart.y != HighEnd.y){

                transform.position +=  new Vector3(1f, 0, 0) * Yspeed * Time.deltaTime;

                if(transform.position.y >= HighEnd.y && bounce)
                {
                    Yspeed *= -1;
                    bounce = false;
                }
                if(transform.position.y <= LowStart.y && !bounce)
                {
                    Yspeed *= -1;
                    bounce = true;
                }
   
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


/* 
 add : 

 public bool GetIsFalling()
  {
  return isFalling;
  }

 to PlayerMovement.cs
 */