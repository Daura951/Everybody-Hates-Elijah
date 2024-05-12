using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public int facingDir { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public AnimationToStateMachine animToState { get; private set; }

    public EnemyHealth health;


    [SerializeField] private Transform wallCheckTF;
    [SerializeField] private Transform ledgeCheckTF;
    [SerializeField] private Transform playerCheckTF;
    [SerializeField] private Transform groundCheckTF;

    private float curHealth;

    public float[] stats { get; private set; }

    private Vector2 velocitySpace;

    private Transform playerTF;

    public Hit hit { get; private set; }

    public virtual void Start()
    {
        facingDir = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animToState = GetComponent<AnimationToStateMachine>();
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        hit = GetComponent<Hit>();

        stateMachine = new FiniteStateMachine();
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
        velocitySpace.Set(facingDir * velocity, rb.velocity.y);
        rb.velocity = velocitySpace;
    }

    //public virtual void 

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheckTF.position, transform.right, entityData.wallCheckDist, entityData.whatIsGround);
    }

    public virtual bool checkLedge()
    {
        return Physics2D.Raycast(ledgeCheckTF.position, Vector2.down, entityData.ledgeCheckDist, entityData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheckTF.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void Damage()
    {
        health.TakeDamage(stats[0]);
    }

    public virtual void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0f, 180, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white; //White is for detecting walls and ledges
        Gizmos.DrawLine(wallCheckTF.position, wallCheckTF.position + (Vector3)(Vector2.right * facingDir * entityData.wallCheckDist));
        Gizmos.DrawLine(ledgeCheckTF.position, ledgeCheckTF.position + (Vector3)(Vector2.down * entityData.ledgeCheckDist));
        Gizmos.color = Color.red; //Red is the point at which player is in range for attack!
        Gizmos.DrawWireSphere(playerCheckTF.position + (Vector3)(transform.right * entityData.closeRangeAttackDist), .2f);
        Gizmos.color = Color.green; //Green is min range for awareness
        Gizmos.DrawWireSphere(playerCheckTF.position + (Vector3)(transform.right * entityData.minAgroDist), .2f);
        Gizmos.color = Color.blue; //Blue is Max awarness range!
        Gizmos.DrawWireSphere(playerCheckTF.position + (Vector3)(transform.right * entityData.maxAgroDist), .2f);
    }

    public virtual bool checkPlayerInMinAgroRange() 
    {
        return Physics2D.Raycast(playerCheckTF.position, transform.right, entityData.minAgroDist, entityData.whatIsPlayer);
    }

    public virtual bool checkPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheckTF.position, transform.right, entityData.maxAgroDist, entityData.whatIsPlayer);
    }

    public virtual bool checkPlayerInCloseRangeAttack()
    {

        return Physics2D.Raycast(playerCheckTF.position, transform.right, entityData.closeRangeAttackDist, entityData.whatIsPlayer);
    }

    public virtual void GetHit(float knockBack, float angle)
    {

        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * knockBack;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * knockBack;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent));
        Damage();
    }

    public virtual void Despawn()
    {
        Destroy(this.gameObject);
    }


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox" && !(collision.gameObject.name == "StickyHandHitbox") && collision.gameObject.name != "Grab Hiitbox")
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().GetCurrentStats();
            GetHit(stats[2], stats[1]);
           
        }
    }
}
