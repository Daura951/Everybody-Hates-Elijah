using System;
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

    public float[] stats { get; private set; }

    private Vector2 velocitySpace;

    public bool isMale = true;
    
    public GameObject playerGO;

    public bool isGrabbed;

    public bool isPummeled;

    public int pummelFactor = 0;

    public BladeBound BB;

    public bool isInCutscene;

    public bool[] whichThrow = { false, false, false, false };
    public virtual void Start()
    {
        facingDir = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animToState = GetComponent<AnimationToStateMachine>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        BB = playerGO.GetComponent<BladeBound>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if(BB.GetIsInBladeBound())
        {
            anim.SetFloat("speed", .5f);
        }
        else
        {
            anim.SetFloat("speed", 1);
        }

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
        return Physics2D.Raycast(ledgeCheckTF.position, Vector2.down, entityData.ledgeCheckDist, entityData.whatIsGround) || Physics2D.Raycast(ledgeCheckTF.position, Vector2.down, entityData.ledgeCheckDist,entityData.endLedge);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheckTF.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void Damage()
    {

        health.TakeDamage(stats[0]);
        playerGO.GetComponent<PlayerAttack>().Combo();
        if (!BB.GetIsInBladeBound())
        {
            BB.AddHit();
        }
        else
        {
            BB.AddFreezeHit();
        }
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

        if (!BB.isFrozen)
        {
            rb.AddForce(new Vector2(XComponent, YComponent));
        }
            Damage();
    }
    public virtual void GetGrabbed()
    {
        isGrabbed = true;
        var playerAnimator = playerGO.GetComponent<Animator>();
        var playerMovement = playerGO.GetComponent<PlayerMovement>();
        var playerAttack = playerGO.GetComponent<PlayerAttack>();
        var playerTransform = playerGO.GetComponent<Transform>();

        playerAnimator.SetBool("hasGrabbedEnemy", true);

        if (playerMovement.isLeft)
        {
            // Ensure the grabbed character is facing the same direction as the player
            if (facingDir == -1)
            {
                print("flip left!");
                Flip();
            }
            this.transform.position = new Vector2(playerTransform.position.x - playerAttack.grabOffset.x, playerTransform.position.y + playerAttack.grabOffset.y);
        }
        else
        {
            // Ensure the grabbed character is facing the same direction as the player
            if (facingDir == 1)
            {
                print("flip right!");
                Flip();
            }
            this.transform.position = new Vector2(playerTransform.position.x + playerAttack.grabOffset.x, playerTransform.position.y + playerAttack.grabOffset.y);
        }

    }

    public virtual void getThrown()
    {
        var playerTransform = playerGO.GetComponent<Transform>();

        for (int i = 0; i < whichThrow.Length; i++)
        {
            if (whichThrow[i])
            {
                if (playerGO.gameObject.GetComponent<PlayerMovement>().isLeft)
                {
                    this.transform.position = new Vector2((playerTransform.position.x - playerTransform.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].x), playerTransform.position.y + playerTransform.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].y);  
                }

                else
                {
                    this.transform.position = new Vector2(playerTransform.position.x + playerTransform.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].x, playerTransform.position.y + playerTransform.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].y);
                }
                print("WHICH THROW: " + i);
            }
            whichThrow[i] = false;
        }
        NotCurrentlyGrabbed();
        isGrabbed = false;
        
    }

    public virtual void GetPummeled()
    {
        health.TakeDamage(stats[0]);
        pummelFactor++;
        isPummeled = true;
    }

    public virtual void Despawn()
    {
        PlayerPrefs.SetInt(“enemiesKilled”, PlayerPrefs.GetInt(“enemiesKilled”) + 1);
        Destroy(this.gameObject);
    }


    public virtual void CurrentlyGrabbed()
    {
        playerGO.GetComponent<PlayerAttack>().currentlyGrabbedEntity = this;
    }

    public virtual void NotCurrentlyGrabbed()
    {
        playerGO.GetComponent<PlayerAttack>().currentlyGrabbedEntity = null;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox" && !(collision.gameObject.name == "StickyHandHitbox") && collision.gameObject.name != "Grab Hitbox" && collision.gameObject.name != "Pummel Hitbox")
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().GetCurrentStats();
            GetHit(stats[2], stats[1]);

        }

        if (collision.gameObject.name == "Grab Hitbox")
        {
            GetGrabbed();
        }

        if(collision.gameObject.name == "Pummel Hitbox")
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().GetCurrentStats();
            GetPummeled();
        }
    }

}
