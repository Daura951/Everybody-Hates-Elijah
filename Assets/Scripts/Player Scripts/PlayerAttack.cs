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

    [Header("Attack variables")]
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

    private int currentHitBox = 0;
    private int[] attackIndexes;
    private string[] attacksData;

    private bool stunned;


    private string FilePath;
    string[] Line;


    public bool OnLadder;

    public bool ASideB = false, SideBS = false;
    public float distance, Speed = 20;
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
    public AudioSource Combofx;
    public AudioClip[] ComboClip;
    public AudioSource[] Hand;
    public AudioSource[] Grunts;
    public AudioSource[] SpecialSource;
    public AudioClip[] SmallGrunts;
    public AudioClip[] MedGrunts;
    public AudioClip[] LargeGrunts;
    public AudioClip[] StrongGrunts;
    private bool strongGrunt = false;
    public AudioClip[] SpecialSounds;


    public float ComboTimer = 3f;
    private float comboScore;
    private float timer, comboTimerStored;






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
        comboTimerStored = ComboTimer;

        attackIndexes = new int[Line.Length];
        attacksData = new string[Line.Length];

        for(int i = 0; i < Line.Length-1; i++)
        {
            attackIndexes[i] = int.Parse(Line[i+1].Split(" ")[0]);

            string attackDataStr = "";
            attackDataStr = Line[i+1].Split(" ")[1];

            for(int j = 2; j < Line[i+1].Split(" ").Length; j++)
            {
                attackDataStr += " " + Line[i+1].Split(" ")[j];
                
            }

            attacksData[i] = attackDataStr;

            //print(attackIndexes[i] + "( " + attacksData[i]+")");

        }
    }

    private void Update()
    {

        DepleteComboTimer();
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
        if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && !stunned && !isGrab && !anim.GetBool("isLaying"))
        {
            isSpecial = false;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && anim.GetBool("Idle") && !playerMovement.isInAir && !stunned && !isGrab)
        {
            //UTilt
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && anim.GetBool("Crouch") && !playerMovement.isInAir && !stunned && !isGrab)
        {
            //DTilt
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && !playerMovement.isInAir && !stunned && !anim.GetBool("Running") && !isGrab)
        {
            //FTilt
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !Input.GetButtonDown("Grab") && !isAttacking && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            //NAir
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            //UAir
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            //DAir
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)) && playerMovement.isInAir && !stunned && !isExecutedOnce && !isGrab)
        {
            //FAir
            isAttacking = true;
        }

        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire3")) && !isAttacking && playerMovement.isInAir && !stunned && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)) && !isGrab)
        {
            //FAir
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && anim.GetBool("Running") && !isSpecial && !isGrab)
        {
            //Dash
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire2") && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f && !stunned && !anim.GetBool("isLaying"))
        {
            //NSpecial
            isSpecial = true;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") < 0f && !stunned && !isExecutedOnce)
        {
            //DSpecial
            isAttacking = true;
            isSpecial = true;
            bypassMoveBlock = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") > 0f && !stunned && !isExecutedOnce)
        {
            //USpecial
            isAttacking = true;
            bypassMoveBlock = true;
            isSpecial = true;
            isInHelpless = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") == 0 && !stunned && !isExecutedOnce)
        {
            //FSpecial
            isAttacking = true;
            isSpecial = true;
            SideBS = true;
            rb.gravityScale = 0;
            playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, 0);
        }

        else if (Input.GetButtonDown("Grab") && !isAttacking && Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0 && !stunned && !isExecutedOnce && !playerMovement.isInAir && !stunned)
        {
            //Grab
            isAttacking = true;
            isSpecial = false;
            isGrab = true;

        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && !isAttacking && anim.GetBool("hasGrabbedEnemy") == true && !playerMovement.isInAir && !stunned && isGrab)
        {
            //Pummel
            isSpecial = false;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)) && isGrab)
        {
            //BThrow
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && ((playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)) && isGrab)
        {
            //FThrow
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") < 0 && !isAttacking && Input.GetAxisRaw("Horizontal") == 0 && isGrab)
        {
            //DThrow
            isGrab = true;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") > 0 && !isAttacking && Input.GetAxisRaw("Horizontal") == 0 && isGrab)
        {
            //UThrow
            isGrab = true;
            isAttacking = true;
        }



        else if (Input.GetButton("Fire3") && !strongDone && !isAttacking && Input.GetAxisRaw("Vertical") == 0 && !playerMovement.isInAir && !stunned && !isSpecial && !isGrab && !anim.GetBool("isLaying"))
        {
            //FStrong
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
            //UStrong
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
            //DStrong
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
            //Strong Held
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
            //Strong Release
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

    void DepleteComboTimer()
    {
        if (comboScore > 0)
        {

            if (timer >= ComboTimer)
            {
                timer = ComboTimer;
            }

            if (timer == ComboTimer)
            {
                print("Combo score: " + comboScore);
                comboScore = 0;
                ComboTimer = comboTimerStored;
            }

            else timer += Time.deltaTime;
        }

    }


    public void SideBMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, 25f * Time.deltaTime);
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


    public void parseAttack(int index)
    {
        //print(attacksData[index]);
        //print(attackIndexes[index]);

        string[] statSplit = attacksData[index].Split(" ");

        hitBoxes[attackIndexes[index]].SetActive(true);
        currentHitBox = attackIndexes[index];
        //print(statSplit[0]);

        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 

        float xDir = 0;
        float yDir = 0;

        if(statSplit.Length > 5 && (statSplit[5] != null || statSplit[6] !=null))
        {
             xDir = float.Parse(statSplit[5]); //XDir
             yDir = float.Parse(statSplit[6]); //YDir
        }

        
        if(statSplit[0].Contains("USpecial"))
        {
            USpecial(xDir, yDir);
        }

        else if(statSplit[0].Contains("Dash"))
        {
            Dash();
        }

        else if(statSplit[0].Contains("Strong"))
        {
            Strong();
        }

        else if(statSplit[0].Contains("DThrow"))
        {
            DThrow();
        }

        else if(statSplit[0].Contains("Throw"))
        {
            Throw();
        }

    }

    //Some attacks require addition functionality upon hitbox appearance within parsing. The functions below are responsible for that

    public void Dash()
    {
        playerMovement.dashDisable = true;  //So that if the player holds dash key, the won't move as soon as dash finishes
    }


    public void Strong()
    {
        strongDamage = 0.0f; //Reset Strong damage so it doesn't stack
    }

    public void USpecial(float xDir, float yDir)
    {
        if (!isExecutedOnce)
        {
            Physics2D.gravity = new Vector2(0, 0);
            playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, 0);
            isExecutedOnce = true;
           // AS.PlayOneShot(playerMovement.FXjump);
        }

        playerMovement.rb.AddForce(new Vector2(xDir, yDir));
    }

    public void Throw()
    {
        currentlyGrabbedEnemy.isGrabbed = false; //Make sure the currently grabbed enemy is unlocked
    }

    public void DThrow()
    {
        if (currentlyGrabbedEnemy != null)
        {
            currentlyGrabbedEnemy.isGrabbed = false;
            playerMovement.transform.position = new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y + playerDThrowOffset.y); //Makes sure that the enemy position remains upon multiple throw hitboxes
        }
    }

    //OnCollision / OnTrigger functions

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



    //Additional functions that animation events / other scripts call

    //Despawns a hitbox
    public void DespawnHitBox()
    {
        if (currentHitBox == 16)
        {
            isSpecial = false;
        }

        hitBoxes[currentHitBox].SetActive(false);
    }

    //Overload. Despawns hitbox based on index
    public void DespawnHitBox(int hitBoxIndex)
    {
        if (hitBoxIndex == 16)
        {
            isSpecial = false;
        }

        hitBoxes[hitBoxIndex].SetActive(false);
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

    public void StrongReset()
    {
        strongGrunt = false;
    }

    public void Slap(int i)
    {
        if (!Hand[i].isPlaying)
            Hand[i].Play();
    }

    public void Grunt(int i)
    {
        float j = UnityEngine.Random.Range(0, 2);
        int k = UnityEngine.Random.Range(0, (LargeGrunts.Length - 1) / 2);

        if (i == 0 && !Grunts[i].isPlaying && j == 0)
        {
            int l = UnityEngine.Random.Range(0, SmallGrunts.Length - 1);
            Grunts[i].PlayOneShot(SmallGrunts[l], 1f);
        }
        if (i == 1 && !Grunts[i].isPlaying && j == 0)
        {
            int l = UnityEngine.Random.Range(0, MedGrunts.Length - 1);
            Grunts[i].PlayOneShot(MedGrunts[l], 1f);
        }
        if (i == 2 && !Grunts[i].isPlaying && j == 0)
        {
            strongGrunt = true;
            Grunts[i].PlayOneShot(LargeGrunts[k * 2], 1f);


        }
        if (strongGrunt && i == 3 && !Grunts[i - 1].isPlaying)
        {
            strongGrunt = false;
            Grunts[i - 1].PlayOneShot(LargeGrunts[(k * 2) + 1], 1f);
        }

        if (i == 2 && !Grunts[i + 1].isPlaying && j == 1)
        {
            int l = UnityEngine.Random.Range(0, StrongGrunts.Length - 1);
            Grunts[i + 1].PlayOneShot(StrongGrunts[l], 1f);


        }

    }

    public void SpecialSoud(int i)
    {
        if (!SpecialSource[i].isPlaying)
            SpecialSource[i].PlayOneShot(SpecialSounds[i], 1f);

    }


    public void Combo()
    {

        timer = 0f;
        comboScore++;

        if (comboScore < 21)
            ComboTimer += .1f;

        if (comboScore == 1 || comboScore % 10 == 0)
        {
            int j = (int)comboScore / 10;

            if (comboScore == 1)
                Combofx.PlayOneShot(ComboClip[0], 1f);
            else
                Combofx.PlayOneShot(ComboClip[j], 1f);
        }
    }


    //Getters and setters

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
}
