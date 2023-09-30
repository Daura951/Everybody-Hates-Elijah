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



    public struct Attack
    {
        public float attackDistance;
        public string attackName;

        public Attack(float distance, string name)
        {
            this.attackDistance = distance;
            this.attackName = name;
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
        attacks = new Attack[2];
        attacks[0] = new Attack(2f, "punch");
        attacks[1] = new Attack(1f, "kick");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && canFollow && !ES.getIsStunned() && !H.getIsStunned())
        {
            FollowPath();

            if (rb.velocity.x == 0)
            {
                anim.SetBool("isRunning", false);
            }
            else anim.SetBool("isRunning", true);

        }
        else
        {
            print("uwu");
            anim.SetBool("isRunning", false);
        }
    }

    private void UpdatePath()
    {

        for(int i = 0; i < attacks.Length; i++)
        {
            if (Vector3.Distance(target.position, transform.position) <= attacks[i].attackDistance && Vector3.Distance(target.position, transform.position) > attacks[i].attackDistance - .5f && !ES.getIsStunned() && !H.getIsStunned())
            {
                print(attacks[i].attackName);
                isAttacking = true;
                break;
            }

            else isAttacking = false;
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
}