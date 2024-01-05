using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Target : MonoBehaviour
{
    public Animator anim;

    [Header("Pathfinding")]
    public Transform target;
    public float activationDistance = 50.0f;
    public float targettingDistance = 100.0f;
    public float pathUpdateSeconds = 0.5f;


    [Header("Physics")]
    public float speed = 200.0f;
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightReq = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool canFollow = true;
    public bool canJump = true;
    public bool canChangeDirection = true;

    private Path path;
    private int currentWayPoint = 0;
    private float tfBeforeJump;
    public RaycastHit2D isGrounded;
    public bool isAwake = false;
    private bool isAttacking = false;
    Seeker seeker;
    Rigidbody2D rb;
    EnemyStun ES;
    Hit H;
    private EnemyHealth health;


    [Header("Hitbox")]
    public GameObject[] hitboxes;

    [Header("Grabs")]
    public bool isGrabbed = false;
    public bool grabee = false;
    private float grabTimer = 0f;
    public bool[] whichThrow = { false, false, false, false };
    //{UThrow, FThrow, BThrow, BThrow}


    public struct Attack
    {
        public float attackDistance;
        public string attackName;
        public int damage;

        public Attack(float distance, string name, int damage)
        {
            this.attackDistance = distance;
            this.attackName = name;
            this.damage = damage;
        }
    }

    private Attack[] attacks;


    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        ES = GetComponent<EnemyStun>();
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        H = GetComponent<Hit>();
        health = GetComponent<EnemyHealth>();
        attacks = new Attack[2];
        attacks[0] = new Attack(2f, "isPunching", 10);
        attacks[1] = new Attack(1f, "isKicking", 5);

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && canFollow && !ES.getIsStunned() && !H.getIsStunned() && health.GetHealth() > 0 && !isGrabbed)
        {
            FollowPath();

            if (rb.velocity.x == 0)
            {
                anim.SetBool("isRunning", false);
            }
            else anim.SetBool("isRunning", true);

        }
        else if(!isGrabbed)
        {
            anim.SetBool("isRunning", false);

            if(health.GetHealth() <=0)
            {
                anim.SetBool("isDead", true);
            }

        }

        if(isGrabbed)
        {

            for(int i = 0; i < hitboxes.Length; i++)
            {
                DespawnHitbox(i);
            }

            if (grabTimer < target.gameObject.GetComponent<PlayerAttack>().maxGrabTime && health.health > 0)
            {
                grabTimer += Time.deltaTime;
                GetComponent<Animator>().SetTrigger("isGrabbed 0");
                GetComponent<Animator>().SetBool("isGrabbed", true);

                for (int i = 0; i < whichThrow.Length; i++)
                {
                    if (whichThrow[i])
                    {
                        if (target.gameObject.GetComponent<PlayerMovement>().isLeft)
                        {
                            this.transform.position = new Vector2((target.position.x - target.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].x), target.position.y + target.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].y);
                            transform.localScale = new Vector3(1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                        }

                        else
                        {
                            this.transform.position = new Vector2(target.position.x + target.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].x, target.position.y + target.gameObject.GetComponent<PlayerAttack>().throwingOffsets[i].y);
                            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                        }
                    }
                    whichThrow[i] = false;
                }
            }

            else isGrabbed = false;
        }

        else if(grabee && !isGrabbed)
        {
            grabTimer = 0;
            GetComponent<Animator>().ResetTrigger("isGrabbed 0");
            GetComponent<Animator>().SetBool("isGrabbed", false);
            target.gameObject.GetComponent<Animator>().SetBool("hasGrabbedEnemy", false);
            this.transform.parent = null;
            rb.gravityScale = 1;
            grabee = false;
            grabTimer = 0;
            target.gameObject.GetComponent<PlayerAttack>().isGrab = false;
            target.gameObject.GetComponent<PlayerAttack>().currentlyGrabbedEnemy = null;
        }
    }

    private void UpdatePath()
    {
        if (!isGrabbed)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                if (Vector3.Distance(target.position, transform.position) <= attacks[i].attackDistance && Vector3.Distance(target.position, transform.position) > attacks[i].attackDistance - .5f && !ES.getIsStunned() && !H.getIsStunned())
                {
                    anim.SetBool(attacks[i].attackName, true);
                    isAttacking = true;
                    break;
                }

                else
                {
                    anim.SetBool(attacks[i].attackName, false);
                }
                isAttacking = false;
            }
        }
        //If we can follow the player and the player is in distance and if we have calculated the current path
        if(canFollow && TargetInDistance() && seeker.IsDone() && !isAttacking)
        {
            //Get a new path
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void FollowPath()
    {
        if(path == null)
        {
            return;
        }

        //If we reached path
        if(currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }
        
        //If we collided
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset); //Gets offset for raycast
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        //Calculate Direction
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;


            //Jump time!
            if (canJump && isGrounded.collider != null && isGrounded.collider.tag == "Platform")
            {
                if (direction.y > jumpNodeHeightReq)
                {
                    rb.AddForce(Vector2.up * speed * jumpModifier);
                }
            }

            rb.AddForce(new Vector2(force.x, 0.0f));
 

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        //If we can change direction, change based on the forces provided
        if(canChangeDirection)
        {
            if(rb.velocity.x > .05f)
            {
                transform.localScale = new Vector3(1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

    }

    private bool TargetInDistance()
    {
        if (isAwake)
        {
            return Vector2.Distance(transform.position, target.transform.position) < targettingDistance;
        }
        else
        {
            if(Vector2.Distance(transform.position, target.transform.position) < activationDistance)
            {
                isAwake = true;
            }

            return Vector2.Distance(transform.position, target.transform.position) < activationDistance;
        }
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    public bool GetIsLeft()
    {
        if( transform.localScale.x > 0)
        return true;

        else
        return false;
    }

    public void DespawnHitbox(int HBIndex)
    {
        hitboxes[HBIndex].SetActive(false);
    }

    public void Punch()
    {
        hitboxes[0].SetActive(true);
    }

    public void Kick()
    {
        hitboxes[1].SetActive(true);
    }

    public void DespawnEnemy()
    {
        for(int i = 0; i < hitboxes.Length; i++)
        {
            DespawnHitbox(i);
        }
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Grab Hitbox")
        {
            isGrabbed = true;
            grabee = true;

            target.gameObject.GetComponent<Animator>().SetBool("hasGrabbedEnemy", true);
            target.gameObject.GetComponent<PlayerAttack>().currentlyGrabbedEnemy = this;
            rb.velocity = new Vector2(0, 0);
            GetComponent<Animator>().SetTrigger("isGrabbed 0");
            GetComponent<Animator>().SetBool("isGrabbed", true);

            if (target.gameObject.GetComponent<PlayerMovement>().isLeft)
            {
                this.transform.position = new Vector2((target.position.x - target.gameObject.GetComponent<PlayerAttack>().grabOffset.x), target.position.y + target.gameObject.GetComponent<PlayerAttack>().grabOffset.y);
                transform.localScale = new Vector3(1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            else
            {
                this.transform.position = new Vector2(target.position.x + target.gameObject.GetComponent<PlayerAttack>().grabOffset.x, target.position.y + target.gameObject.GetComponent<PlayerAttack>().grabOffset.y);
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            this.transform.parent = target;
            rb.gravityScale = 0;
        }
    }
}