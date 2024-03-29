using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private Rigidbody2D rb;
    Enemy_Target ET;
    EnemyHealth H;
    public GameObject Player;

    public bool isHit = false, isLeft = false, Stunned;
    private float timer;
    private float[] stats;



    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       ET = GetComponent<Enemy_Target>();
       H = GetComponent<EnemyHealth>();

        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            Player.GetComponent<PlayerAttack>().Combo();
            timer = stats[3];
            // Debug.Log(timer);
            H.TakeDamage(stats[0]);
            GetHit(stats[2], stats[1]);
        }

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
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    private void GetHit(float knockBack, float angle)
    {

        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * knockBack;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * knockBack;

        if(isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent));

        isHit = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Hitbox" && !(collision.gameObject.name == "StickyHandHitbox") && collision.gameObject.name != "Grab Hiitbox")
        {
            stats = Player.GetComponent<PlayerAttack>().GetCurrentStats();
            if(stats[2] !=0)
            rb.velocity = new Vector2(0,0);
            isHit = Stunned = true;
            isLeft = Player.GetComponent<PlayerMovement>().GetIsLeft();

            if(collision.gameObject.name== "Pummel Hitbox")
            {
                GetComponent<Animator>().Play("Grab Hurt");
            }
        }

        else if(collision.gameObject.tag == "Hitbox" && collision.gameObject.name== "StickyHandHitbox" && collision.gameObject.name!="Grab Hiitbox")
        {
            print("Gotcha!!!!");
            stats = Player.GetComponent<PlayerAttack>().GetCurrentStats();
            if(stats[2] !=0)
            rb.velocity = new Vector2(0,0);

            isHit = Stunned = true;
            isLeft = Player.GetComponent<PlayerMovement>().GetIsLeft();
            Player.GetComponent<PlayerAttack>().isSticked = true;
            Player.GetComponent<PlayerAttack>().stickyHand.GetComponent<StickyHand>().goBack = true;
            Player.GetComponent<PlayerAttack>().hitBoxes[6].GetComponent<StickyHand>().goBack = true;
            Player.GetComponent<PlayerAttack>().hitBoxes[6].GetComponent<CircleCollider2D>().enabled = false;

        }

        if(collision.gameObject.name=="Dash  Hitbox")
        {
            collision.transform.parent.GetComponent<PlayerMovement>().rb.velocity = new Vector2(0, 0);
        }

        rb.velocity = new Vector2(0, 0);
    }

    public bool getIsStunned()
    {
        return Stunned;
    }
}
