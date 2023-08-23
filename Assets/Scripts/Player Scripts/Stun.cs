using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool Stunned;
    private bool isLeft;
    private float timer;
    private Rigidbody2D rb;
    PlayerMovement PM;
    Health H;
    
    private Stun_Info SI;
    private float[] SIDAKT;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       PM = GetComponent<PlayerMovement>();
       H = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {   
        isLeft = PM.GetIsLeft();

        if(Stunned)
        {
           if(timer > 0)
           {
            timer-= Time.deltaTime;
           }
           else
            timer = 0;
           if(timer == 0)
            Stunned=false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
      if(col.gameObject.GetComponent<Stun_Info>())
      {
       if(!Stunned)
       rb.velocity = new Vector2(0,0);
            
       Stunned = true;
      

       SI = col.gameObject.GetComponent<Stun_Info>();
       SIDAKT = SI.GetDAKTInfo();

       timer = SIDAKT[3];
       /* 
       print("Damage " + SIDAKT[0]);
       print("Angle " + SIDAKT[1]);
       print("Knockback " + SIDAKT[2]);
       print("Time " + SIDAKT[3]);
       */
       H.TakeDamage(SIDAKT[0]);
       GetHit(SIDAKT[1], SIDAKT[2]);
      }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
      if(col.gameObject.GetComponent<Stun_Info>())
      {
       if(!Stunned)
       rb.velocity = new Vector2(0,0);
            
       Stunned = true;
      

       SI = col.gameObject.GetComponent<Stun_Info>();
       SIDAKT = SI.GetDAKTInfo();

       timer = SIDAKT[3];
       /* 
       print("Damage " + SIDAKT[0]);
       print("Angle " + SIDAKT[1]);
       print("Knockback " + SIDAKT[2]);
       print("Time " + SIDAKT[3]);
       */
       H.TakeDamage(SIDAKT[0]);
       GetHit(SIDAKT[1], SIDAKT[2]);
      }
    }

    private void GetHit(float angle , float Kb)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * Kb;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * Kb;

        

        if(!isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent) , ForceMode2D.Impulse);
    }

    public bool getIsStunned()
    {
        return Stunned;
    }
}
