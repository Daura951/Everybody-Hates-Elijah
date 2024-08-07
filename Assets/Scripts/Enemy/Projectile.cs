using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected AttackDetails attackDetails;

    protected float speed;
    protected Rigidbody2D rb;
    protected float travelDist;
    protected float xStartPos;
    protected bool isGoingBack;
    protected bool hasHitGround;

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform damagePos;

    [SerializeField] protected float damageRadius;


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        rb.velocity = transform.right * speed;

        xStartPos = transform.position.x;
    }

    public virtual void FixedUpdate()
    {

        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsGround);
            Collider2D enemyHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsEnemy);

            if (damageHit)
            {
                Destroy(gameObject);
            }
            if (groundHit)
            {
                hasHitGround = true;
                Destroy(this);
            }

            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDist && !isGoingBack)
            {
                isGoingBack = true;
                rb.velocity = -transform.right * speed;
            }

        }

    }

    public virtual void FireProjectile(float speed, float travelDist, float damage)
    {
        this.speed = speed;
        this.travelDist = travelDist;
        attackDetails.damage = damage;
    }

    public virtual void OnDrawGizmos()
    {
        
    }
}
