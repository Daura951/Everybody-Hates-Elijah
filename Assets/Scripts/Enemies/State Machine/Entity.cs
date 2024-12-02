using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public int facingDir { get; private set; }

    private Vector2 velocityWorkspace;

    [SerializeField]
    private Transform wallCheckTf, ledgeCheckTf, playerCheckTf, groundCheckTf;

    public bool isAnimationFinished = false;

    private float currentHealth;
    public int lastDamageDir { get; private set; }

    public AttackDetails currentHit;
    public bool isStunned;
    private Player player;

    public bool isDead = false;

    private EnemyHealthbar healthbar;


    public virtual void Start()
    {
        stateMachine = new FiniteStateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = entityData.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthbar = GetComponentInChildren<EnemyHealthbar>();
        facingDir = 1;
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDir * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, float angle, int direction)
    {
        rb.velocity = Vector2.zero;
        //print(velocity + " " + angle);

        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180));
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180));

        Vector2 angleVec = new Vector2(XComponent, YComponent);
        velocityWorkspace.Set(angleVec.x * velocity * direction, angleVec.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheckTf.position, transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheckTf.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheckTf.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheckTf.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheckTf.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheckTf.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckIsInLaunchVelocity()
    {
        return rb.velocity.magnitude >= entityData.launchVelocityThreshold;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.Damage;
        healthbar.UpdateHealthbar(currentHealth, entityData.maxHealth);
        lastDamageDir = player.transform.position.x > transform.position.x ? -1 : 1;
        SetVelocity(attackDetails.Knockback, attackDetails.Angle, lastDamageDir);
        isStunned = true;

        if(currentHealth <= 0)
        {
            isDead = true;
        } 

    }

    public virtual void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheckTf.position, wallCheckTf.position + (Vector3)(Vector2.right*facingDir*entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheckTf.position, ledgeCheckTf.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheckTf.position + (Vector3)(transform.right * entityData.closeRangeActionDistance), 0.2f);

        Gizmos.DrawWireSphere(playerCheckTf.position + (Vector3)(transform.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheckTf.position + (Vector3)(transform.right * entityData.maxAgroDistance), 0.2f);
    }

    public void EndAttack()
    {
        isAnimationFinished = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hitbox")
        {
            currentHit = player.GetAttackManager().findByHitbox(collision.gameObject.name);
            Damage(currentHit);
        }
    }
}
