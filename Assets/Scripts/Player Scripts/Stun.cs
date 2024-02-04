using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool Stunned;
    private bool isLeft;
    private float timer;
    private Rigidbody2D rb;
    private Animator anim;
    PlayerMovement PM;
    Health H;
    
    private Stun_Info SI;
    private float[] SIDAKT;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       PM = GetComponent<PlayerMovement>();
       anim = GetComponent<Animator>();
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
            if (timer == 0)
            {
                Stunned = false;
                rb.gravityScale = 1f;
            }
            anim.SetBool("Stunned",Stunned);
            Physics2D.gravity = new Vector2(0, -9.81f);
            rb.gravityScale = 2;

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
      if(col.gameObject.GetComponent<Stun_Info>())
      {
       if(!Stunned)
       rb.velocity = new Vector2(0,0);
            
       Stunned = true;
       anim.SetBool("Stunned",Stunned);
       anim.Play("Stunned");
      
       if(PlayerAttack.attackInstance.ASideB)
       PlayerAttack.attackInstance.ASideB = false;

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
            anim.SetBool("Stunned",Stunned);
            anim.Play("Stunned");
      

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

            if(gameObject.tag=="Player")
            {
                gameObject.GetComponent<PlayerMovement>().ChangeHealth();
                gameObject.GetComponent<PlayerMovement>().GetAnim().SetBool("isFalling", true);
                gameObject.GetComponent<PlayerMovement>().isInAir = true;
            }


       
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
