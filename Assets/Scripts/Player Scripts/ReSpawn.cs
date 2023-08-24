using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    Health H;
    PlayerMovement PM;
    Rigidbody2D rb; 
    Stun s;
    Animator anim;
    Vector2 ReSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
      H = GetComponent<Health>();
      PM = GetComponent<PlayerMovement>();
      rb = GetComponent<Rigidbody2D>();
      s = GetComponent<Stun>();
      anim = GetComponent<Animator>();
      ReSpawnPoint = new Vector2(PM.transform.position.x , PM.transform.position.y); 
    }

    // Update is called once per frame
    void Update()
    {
        if( H.GetHealth() == 0)
        {
        rb.velocity = new Vector2(0f , 0f);
        PM.transform.position = ReSpawnPoint;

        if(s.Stunned)
        {
            anim.SetBool("Stunned",!s.Stunned);

         s.Stunned = false;
        }

        
        H.ReHeal();
        }
    }

    public void NewCheckPoint(Vector2 c)
    {
        ReSpawnPoint = c ;
    }
}
