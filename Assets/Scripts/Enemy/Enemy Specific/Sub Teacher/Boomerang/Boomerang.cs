using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Projectile
{

    private SubTeacher parentSub;

    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate()
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
                
                Destroy(this);
            }

            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDist && !isGoingBack)
            {
                isGoingBack = true;
                rb.velocity = -transform.right * speed;
            }
            else if (isGoingBack && Mathf.Abs(transform.position.x - xStartPos) < 0.1f)
            {
                Destroy(gameObject);
            }

    }
    public override void FireProjectile(float speed, float travelDist, float damage)
    {
        base.FireProjectile(speed, travelDist, damage);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(damagePos.position, damageRadius);
    }

}
