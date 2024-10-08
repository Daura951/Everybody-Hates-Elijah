using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : MonoBehaviour
{

    [Range(0,2)]
    public float mass = 1.0f;

    [Range(1, 40)]
    public float velocityThreshold;

    [Range(0,1)]
    public float launchVelocityFactor;

    private bool isInLaunch = false;
    
    private Rigidbody2D rb;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.mass = mass;

        if(isInLaunch)
        {
            render.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public virtual void GetHit(PlayerAttackDetails details, bool isRight)
    {

        float XComponent = Mathf.Cos(details.Angle * (Mathf.PI / 180)) * details.Knockback;
        float YComponent = Mathf.Sin(details.Angle * (Mathf.PI / 180)) * details.Knockback;

        if (!isRight)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent));
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude >= velocityThreshold)
        {
            isInLaunch = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            AttackManager atkManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackManager>();
            if(collision.gameObject.name.Contains("Sticky"))
            {
                atkManager.didStickyCollide = true;
            }

            GetHit(atkManager.findByHitbox(collision.gameObject.name), atkManager.GetPlayerState().isRight);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isInLaunch && collision.gameObject.tag.ToLower().Contains("platform"))
        {
            isInLaunch = false;
            render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }


}
