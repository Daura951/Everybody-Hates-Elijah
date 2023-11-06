using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    private Rigidbody2D rb;
    ObjectHealth OH;

    public bool isHit = false, isLeft = false, Stunned;
    private float timer;
    private float[] stats;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OH = GetComponent<ObjectHealth>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isHit && !anim.GetBool("isDead"))
        {
            timer = stats[3];
            Debug.Log(timer);
            OH.TakeDamage(stats[0]);
            anim.Play("TurretDamage", -1, 0);
            GetHit(stats[2], stats[1]);
        }

        if (Stunned)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
                timer = 0;
            if (timer == 0)
                Stunned = false;
        }
    }

    private void GetHit(float knockBack, float angle)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * knockBack;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * knockBack;

        if (isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent));

        isHit = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Hitbox" && !(collision.gameObject.name == "StickyHandHitbox") && collision.gameObject.name != "Grab Hitbox")
        {
            stats = collision.transform.parent.gameObject.GetComponent<PlayerAttack>().GetCurrentStats();
            if (stats[2] != 0)
                rb.velocity = new Vector2(0, 0);
            isHit = Stunned = true;
            isLeft = collision.transform.parent.gameObject.GetComponent<PlayerMovement>().GetIsLeft();
        }

        else if (collision.gameObject.tag == "Hitbox" && collision.gameObject.name == "StickyHandHitbox" && collision.gameObject.name != "Grab Hitbox")
        {
            print("Gotcha!!!!");
            stats = collision.transform.parent.gameObject.GetComponent<PlayerAttack>().GetCurrentStats();
            if (stats[2] != 0)
                rb.velocity = new Vector2(0, 0);

            isHit = Stunned = true;
            isLeft = collision.transform.parent.gameObject.GetComponent<PlayerMovement>().GetIsLeft();
            collision.transform.parent.gameObject.GetComponent<PlayerAttack>().isSticked = true;
            collision.transform.parent.gameObject.GetComponent<PlayerAttack>().stickyHand.GetComponent<StickyHand>().goBack = true;
            collision.transform.parent.gameObject.GetComponent<PlayerAttack>().hitBoxes[6].GetComponent<StickyHand>().goBack = true;
            collision.transform.parent.gameObject.GetComponent<PlayerAttack>().hitBoxes[6].GetComponent<CircleCollider2D>().enabled = false;

        }
    }

    public bool getIsStunned()
    {
        return Stunned;
    }
}
