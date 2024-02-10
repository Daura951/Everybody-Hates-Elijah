using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerAttack : MonoBehaviour
{

    [Header("Objects")]
    public Animator anim;
    public PlayerMovement playerMovement;
    Health H;
    private Ladder Ladder;
    public static PlayerAttack attackInstance;
    Stun S;
    private Rigidbody2D rb;

    public bool isAttacking, isSpecial = false, isSticked = false, isGrab = false;

    public GameObject[] hitBoxes;
    
    public GameObject stickyHand;
    private float[] currentStats;

    public bool strongStarted = false;
    public bool strongDone = false;
    public bool isExecutedOnce = false;
    public bool bypassMoveBlock = false;
    public bool isInHelpless = false;
    public float strongTimer = 0.0f;
    float strongDamage = 0.0f;


    private bool stunned;


    private string FilePath;
    string[] Line;


    public bool OnLadder;

    public bool ASideB = false, SideBS = false;
    public float distance, Speed = 1;
    public Vector3 target;

    [Header("Grabbing")]
    public Vector2 grabOffset;
    public Vector2 playerDThrowOffset;
    public float maxGrabTime;
    public Enemy_Target currentlyGrabbedEnemy;
    public Vector2[] throwingOffsets;

    private int revFSpecialIndex = 0;


    [Header("Shielding")]
    public GameObject Shield;
    ShieldScript SS;
    public bool shielding = false;
    public bool shieldHeld = false;

    [Header("SoundEffects")]
    public AudioSource AS;
    public AudioClip Strong1;
    public AudioClip Strong2;
    public AudioClip Strong3;
    public AudioClip Strong4;
    public AudioClip Punch;






    private void Awake()
    {
        stickyHand.SetActive(false);
        Shield.SetActive(false);
        attackInstance = this;
    }

    private void Start()
    {
        currentStats = new float[4];
        anim = GetComponent<Animator>();
        S = GetComponent<Stun>();
        H = GetComponent<Health>();
        SS = Shield.GetComponent<ShieldScript>();
        rb = GetComponent<Rigidbody2D>();
        FilePath = Application.dataPath + "/ElijahAttackValues.txt";
        Line = File.ReadAllLines(FilePath);
    }

    private void Update()
    {
        stunned = S.getIsStunned();

        if (Ladder != null)
            OnLadder = Ladder.GetOnLadder();


        if (!shielding && SS.ShieldTimer != 0 && !SS.ShieldStun && SS.ActiveOnce)
        {
            SS.ShieldTimer -= Time.deltaTime;
            if (SS.ShieldTimer <= 0)
                SS.ShieldTimer = 0;

        }



        if (playerMovement.GetAnim().GetBool("isGrounded") == true)
        {
            //If we collided despawn the air hitboxes!
            DespawnHitBox(3);
            DespawnHitBox(10);
            DespawnHitBox(11);
            DespawnHitBox(12);
        }

        if (stunned)
        {
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                DespawnHitBox(i);
            }
        }


        if (!OnLadder && !ASideB && !playerMovement.grabbing && !SS.ShieldStun && !H.dead)
            Attack();

        if (ASideB)
            SideBMove();
    }

    void Attack()
    {


        //Control Detection
        if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && !stunned && !isGrab)
        {
            print("Jab");
            isSpecial = false;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && anim.GetBool("Idle") && !playerMovement.isInAir && !stunned && !isGrab)
        {
            print("UTilt");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && anim.GetBool("Crouch") && !playerMovement.isInAir && !stunned && !isGrab)
        {
            print("DTilt");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && !playerMovement.isInAir && !stunned && !anim.GetBool("Running") && !isGrab)
        {
            print("FTilt");
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !Input.GetButtonDown("Grab") && !isAttacking && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            isAttacking = true;
            print("Nair");
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            print("Uair");
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            print("Dair");
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)) && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            print("Fair");
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && playerMovement.isInAir && !stunned && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)) && !isGrab)
        {
            print("Bair");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && anim.GetBool("Running") && !isSpecial && !isGrab)
        {
            print("Dash");
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire2") && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f && !stunned)
        {
            print("NSpecial");
            isSpecial = true;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") < 0f && !stunned && !isExecutedOnce)
        {
            isAttacking = true;
            isSpecial = true;
            print("DSpecial");
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") > 0f && !stunned && !isExecutedOnce)
        {
            isAttacking = true;
            bypassMoveBlock = true;
            isSpecial = true;
            isInHelpless = true;
            print("USpecial");
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") == 0 && !stunned && !isExecutedOnce)
        {
            isAttacking = true;
            isSpecial = true;
            SideBS = true;
            rb.gravityScale = 0;
            playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, 0);

            print("FSpecial");
        }

        else if (Input.GetButtonDown("Grab") && !isAttacking && Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0 && !stunned && !isExecutedOnce && !playerMovement.isInAir && !stunned)
        {
            print("Grab");
            isAttacking = true;
            isSpecial = false;
            isGrab = true;

        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && !isAttacking && anim.GetBool("hasGrabbedEnemy") == true && !playerMovement.isInAir && !stunned && isGrab)
        {
            print("Pummel");
            isSpecial = false;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)) && isGrab)
        {
            print("BThrow");
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)) && isGrab)
        {
            print("FThrow");
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") < 0 && !isAttacking && Input.GetAxisRaw("Horizontal") == 0 && isGrab)
        {
            print("DThrow");
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") > 0 && !isAttacking && Input.GetAxisRaw("Horizontal") == 0 && isGrab)
        {
            print("UThrow");
            isGrab = true;
            isAttacking = true;
        }



        else if (Input.GetButton("Fire3") && !strongDone && !isAttacking && Input.GetAxisRaw("Vertical") == 0 && !playerMovement.isInAir && !stunned && !isSpecial && !isGrab)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("FStrong Startup");
            }
        }

        else if (Input.GetButton("Fire3") && !strongDone && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && !playerMovement.isInAir && !stunned && !isSpecial && !isGrab)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("UStrong Startup");
            }

        }

        else if (Input.GetButton("Fire3") && !strongDone && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && !playerMovement.isInAir && !stunned && !isSpecial && !isGrab)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("DStrong Startup");
            }
        }

        else if (Input.GetButton("Fire3") && !strongDone && strongStarted && isAttacking)
        {
            strongTimer += Time.deltaTime;

            if (strongTimer < 1.2f)
            {
                strongDamage += .001f;
            }

            if (strongTimer >= 2.2f)
            {
                strongDone = true;
            }
        }



        else if ((Input.GetButtonUp("Fire3") || strongDone) && strongStarted && !isSpecial)
        {
            print("Releasing strong");
            strongStarted = false;
            anim.SetBool("isStrong", strongStarted);
            print(strongDamage);
            strongDamage = 0;
            strongTimer = 0;
            strongDone = false;

            if (Input.GetButton("Fire3"))
                strongDone = true;
        }

        else if ((Input.GetButtonUp("Fire3") && strongDone))
        {
            strongDone = false;
        }


        else if ((Gamepad.current.rightTrigger.isPressed || Gamepad.current.leftTrigger.isPressed || Input.GetButtonDown("Shield")) && Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0 && !stunned && !isExecutedOnce && !playerMovement.isInAir && !shielding && !shieldHeld)
        {
            print("Shield");
            shieldHeld = shielding = Shield.GetComponent<SpriteRenderer>().enabled = true;
            anim.Play("ShieldBlock");
            anim.SetBool("isShielding", true);
            Shield.SetActive(true);
            
        }

        if(!Gamepad.current.rightTrigger.isPressed && !Gamepad.current.leftTrigger.isPressed && !Input.GetButton("Shield"))
        {
            shieldHeld = false;
        }


        if((shielding && !shieldHeld) || playerMovement.isInAir || isAttacking || (Input.GetAxisRaw("Horizontal") != 0f && !SS.ShieldStun) || (Input.GetAxisRaw("Vertical") != 0 && !SS.ShieldStun) )
        {
            ShieldOff();
        }

    }

    public void SideBMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == target)
            ASideB = false;
    }

    public void ShieldOff()
    {
        if (!SS.ShieldStun && !S.isAirSpin)
        {
            shielding = false;
            anim.SetBool("isShielding", false);
            Shield.SetActive(false);
        }
      else
          Shield.GetComponent<SpriteRenderer>().enabled = false;
       

    }

    public void Jab1(int L)
    {
        hitBoxes[0].SetActive(true);
        AS.PlayOneShot(Punch);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Jab2(int L)
    {
        hitBoxes[1].SetActive(true);
        AS.PlayOneShot(Punch);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Jab3(int L)
    {
        hitBoxes[2].SetActive(true);
        AS.PlayOneShot(Punch);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Nair(int L)
    {
        hitBoxes[3].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }
    public void DownB(int L)
    {
        hitBoxes[4].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void SideB(int L)
    {
        hitBoxes[5].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void NeutralB(int L)
    {
        hitBoxes[6].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }


    public void UTilt(int L)
    {
        hitBoxes[7].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void FTilt(int L)
    {
        hitBoxes[8].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }


    public void Dash(int L)
    {
        hitBoxes[20].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        playerMovement.dashDisable = true;           //So that if the player holds dash key, the won't move as soon as dash finishes
    }

    public void DTilt(int L)
    {
        hitBoxes[9].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Fair(int L)
    {
        hitBoxes[10].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Uair(int L)
    {
        hitBoxes[11].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Dair(int L)
    {
        hitBoxes[12].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Bair(int L)
    {
        hitBoxes[19].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void FStrong(int L)
    {
        hitBoxes[13].SetActive(true);
        StrongCry();
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1]) * strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]) + (float.Parse(statSplit[3]) * strongDamage); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        strongDamage = 0.0f;
    }

    public void UStrong(int L)
    {
        hitBoxes[14].SetActive(true);
        StrongCry();
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1]) * strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]) + (float.Parse(statSplit[3]) * strongDamage); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        strongDamage = 0.0f;
    }

    public void DStrong(int L)
    {
        hitBoxes[15].SetActive(true);
        StrongCry();
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1]) * strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]) + (float.Parse(statSplit[3]) * strongDamage); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        strongDamage = 0.0f;
    }

    public void USpecial(int L)
    {
        string[] statSplit = Line[L].Split(" ");
        
        hitBoxes[int.Parse(statSplit[7])].SetActive(true);
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        float XDir = float.Parse(statSplit[5]); //XDir
        float yDir = float.Parse(statSplit[6]); //YDir
        playerMovement.jumpAmt = 0;

        if (!isExecutedOnce)
        {
            Physics2D.gravity = new Vector2(0, 0);
            playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, 0);
            isExecutedOnce = true;
           // AS.PlayOneShot(playerMovement.FXjump);
        }

        playerMovement.rb.AddForce(new Vector2(XDir, yDir));
    }

    public void Grab(int L)
    {
        hitBoxes[21].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 


    }

    public void Pummel(int L)
    {
        hitBoxes[22].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }


    public void UThrow(int L)
    {
        hitBoxes[23].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
        currentlyGrabbedEnemy.isGrabbed = false;
    }

    public void FThrow(int L)
    {
        hitBoxes[24].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
        currentlyGrabbedEnemy.isGrabbed = false;
    }

    public void BThrow(int L)
    {
        hitBoxes[25].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
        currentlyGrabbedEnemy.isGrabbed = false;
    }

    public void DThrow(int L)
    {
        string[] statSplit = Line[L].Split(" ");
        hitBoxes[int.Parse(statSplit[7])].SetActive(true);
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 

        if (currentlyGrabbedEnemy != null)
        {
            currentlyGrabbedEnemy.isGrabbed = false;
            playerMovement.transform.position = new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y + playerDThrowOffset.y);
        }
    }

    public void DetectReverseFSpecial()
    {
        revFSpecialIndex = 0;
        if (playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0)
        {
            revFSpecialIndex = 1;
        }
        else if (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            revFSpecialIndex = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            ASideB = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ASideB = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            Ladder = collision.gameObject.GetComponent<Ladder>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            Ladder = null;
            OnLadder = false;
        }
    }


    public void DespawnHitBox(int hitboxIndex)
    {
        if (hitboxIndex == 16)
        {
            isSpecial = false;
        }

        hitBoxes[hitboxIndex].SetActive(false);
    }

    void DespawnStickyHand()
    {
        stickyHand.SetActive(false);
    }

    void SideBDone()
    {
        SideBS = false;
        rb.gravityScale = 1;
    }

    void ActivateSideB(float distance)
    {
        if (revFSpecialIndex != 0)
        {
            distance /= 2;
        }

        ASideB = true;
        if (!playerMovement.GetIsLeft() && revFSpecialIndex == 0 || revFSpecialIndex == 2)
            target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        else
            target = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
    }


    public void SetStrongDone(bool newStrongDone)
    {
        strongDone = newStrongDone;
    }

    public float[] GetCurrentStats()
    {
        return currentStats;
    }

    public bool GetAttacking()
    {
        return isAttacking;
    }

    public void StrongCry()
    {
        float pick = UnityEngine.Random.Range(1, 5);
        Debug.Log(pick);
        if (pick == 1)
            AS.PlayOneShot(Strong1, 1f);
        if (pick == 2)
            AS.PlayOneShot(Strong2, 1f);
        if (pick == 3)
            AS.PlayOneShot(Strong3, 1f);
        if (pick == 4)
            AS.PlayOneShot(Strong4, 1f);
    }
}
