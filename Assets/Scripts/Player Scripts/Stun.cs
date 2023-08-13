using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    private bool Stunned;
    private bool isLeft;
    private float timer;
    private Rigidbody2D rb;
    PlayerMovement PM;
    
    private Stun_Info SI;
    private float[] SITAKD;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       PM = GetComponent<PlayerMovement>();
       //SITAKD = new float[4];
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

    void OnCollisionEnter2D(Collision2D col)
    {
      if(col.gameObject.GetComponent<Stun_Info>())
      {
      if(!Stunned)
      rb.velocity = new Vector2(0,0);
            
      Stunned = true;
      

       SI = col.gameObject.GetComponent<Stun_Info>();
       SITAKD = SI.GetTAKDInfo();

       timer += SITAKD[0];
       print("Time " + SITAKD[0]);
       print("Angle " + SITAKD[1]);
       print("Knockback " + SITAKD[2]);
       print("Damage " + SITAKD[3]); 

       GetHit(SITAKD[1], SITAKD[2]);
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
