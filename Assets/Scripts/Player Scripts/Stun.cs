using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool Stunned;
    public bool isAirSpin = false;
    private bool isLeft;
    private float timer;
    private Rigidbody2D rb;
    private Animator anim;

    private float stunMultiplier;
    private PlayerMovement PM;
    public float elijahGoFarFloat = 10;
    Health H;
    
    private Stun_Info SI;
    private float[] SIDAKT;
    public float DIDegreeRestriction = 18f;

    [SerializeField]
    private float terminalVelocity = 40;

    public static Stun stunInstance;

    PlayerAttack pa;

    private GameObject g;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       PM = GetComponent<PlayerMovement>();
       anim = GetComponent<Animator>();
       H = GetComponent<Health>();
       pa = GetComponent<PlayerAttack>();
       stunInstance = this;
       stunMultiplier = 10 / H.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {   
        isLeft = PM.GetIsLeft();

        if(Stunned)
        {
            PM.setcanApplyAirMovement(false);
           Physics2D.IgnoreLayerCollision(3, 12,true);
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
                Physics2D.IgnoreLayerCollision(3, 12, false);
            }
            anim.SetBool("Stunned",Stunned);
            Physics2D.gravity = new Vector2(0, -9.81f);
            rb.gravityScale = 2;

            anim.SetBool("isGrounded", true);


            if(rb.velocity.x >= terminalVelocity || rb.velocity.x < -terminalVelocity)
            {
                isAirSpin = true;
                anim.SetBool("isAirStunned", isAirSpin);
                anim.SetBool("isLaying", true);
                anim.SetBool("canLayAttack", true);
                PM.isInGetup = true;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<Stun_Info>() && !H.dead)
        {
            if(!Stunned)
            rb.velocity = new Vector2(0,0);
            

            Stunned = true;
            anim.SetBool("Stunned",Stunned);

            int randStun = UnityEngine.Random.Range(1, 4);
            string stunStr = "Stunned";

            if(randStun != 1)
            {
                stunStr += ""+randStun;
            }

            anim.Play(stunStr);
      
            if(PlayerAttack.attackInstance.ASideB)
            PlayerAttack.attackInstance.ASideB = false;

            SI = col.gameObject.GetComponent<Stun_Info>();
            SIDAKT = SI.GetDAKTInfo();




            if (!pa.shielding)
            {
                if (SI is EnvironmentalStun_info)
                {
                    PM.setcanApplyAirMovement(false);
                }

                timer = SIDAKT[3];
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                GetHit(SIDAKT[1], SIDAKT[2]);
                H.TakeDamage(SIDAKT[0]);
            }
            else
            {
                H.TakeDamage(SIDAKT[0] / 2);
            }
            pa.bypassMoveBlock = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Stun_Info>() && !H.dead)
        {
            g = col.gameObject;
            if (!Stunned)
            rb.velocity = new Vector2(0,0);
            
            Stunned = true;
            anim.SetBool("Stunned",Stunned);
            anim.Play("Stunned");

            SI = col.gameObject.GetComponent<Stun_Info>(); //Polymorphism FTW

     
            SIDAKT = SI.GetDAKTInfo();
            H.TakeDamage(SIDAKT[0]);

            if (g.CompareTag("KnockBack") && H.GetHealth() <= 0f)
                SinkDeath();

            else if (!pa.shielding)
            {
                timer = SIDAKT[3];
                GetHit(SIDAKT[1], SIDAKT[2]);
            }

            if(gameObject.tag=="Player")
            {
                gameObject.GetComponent<PlayerMovement>().ChangeHealth();
                gameObject.GetComponent<PlayerMovement>().GetAnim().SetBool("isFalling", true);
                gameObject.GetComponent<PlayerMovement>().isInAir = true;
            }


            pa.bypassMoveBlock = false;
        }
    }

    private void GetHit(float angle , float Kb)
    {


        /*
         * 
         * TODO DI needs more work
        float DIAmount = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * (180 / Mathf.PI);

        if (angle + DIAmount > angle + DIDegreeRestriction)
        {
            angle += DIDegreeRestriction;
        }
        else if (angle + DIAmount < angle - DIDegreeRestriction)
        {
            angle -= DIDegreeRestriction;
        }
        else angle += DIAmount;
        */

        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * Kb;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * Kb;

        float healthWeight = 0;

        if (H.GetHealth() > 0)
        {
            healthWeight = ((H.GetMaxHealth() / H.GetHealth()) * stunMultiplier);
            
        }
        else healthWeight = ((H.GetMaxHealth() / 1f) * stunMultiplier);

        angle += H.GetHealth() < .5 ? 0 : 10;

        if(!isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce((new Vector2(XComponent, YComponent) * healthWeight) , ForceMode2D.Impulse);
    }

    public bool getIsStunned()
    {
        return Stunned;
    }

    public PlayerMovement getPM()
    {
        return PM;
    }

    private void SinkDeath()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -5;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        g.GetComponent<Collider2D>().enabled = false;
        anim.SetBool("Sinking", true);
        int pick = UnityEngine.Random.Range(0, 2);
        if(pick ==0)
        anim.Play("Sinking2");
        else
        anim.Play("Sinking");
    }

    public void SinkDeathPos()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
    }

    public void EnablePit()
    {
        if(g!=null)
        g.GetComponent<Collider2D>().enabled = true;
    }
}
