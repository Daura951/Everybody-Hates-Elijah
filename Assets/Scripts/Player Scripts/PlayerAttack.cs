using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public bool isAttacking = false, isSpecial = false, isSticked = false;
    public static PlayerAttack attackInstance;
    public GameObject[] hitBoxes;
    public GameObject stickyHand;
    private float[] currentStats;
    public PlayerMovement playerMovement;
    public bool strongStarted = false;
    public bool strongDone = false;
    public float strongTimer = 0.0f;
    float strongDamage = 0.0f;

    private string FilePath;
    string[] Line;

    private void Awake()
    {
        stickyHand.SetActive(false);
        attackInstance = this;
    }

    private void Start()
    {
        currentStats = new float[4];
        anim = GetComponent<Animator>();
        FilePath = Application.dataPath +"/ElijahAttackValues.txt";
        Line = File.ReadAllLines(FilePath);
    }

    private void Update()
    {

        //print(isAttacking);
        Attack();
    }

    void Attack()
    {


        //Jab if statement
        if (Input.GetButtonDown("Fire2") && Input.GetAxisRaw("Vertical") == 0 && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir)
        {
            print("Jab");
            isSpecial = false;
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && anim.GetBool("Idle") && !playerMovement.isInAir)
        {
            print("UTilt");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && anim.GetBool("Crouch") && !playerMovement.isInAir)
        {
            print("DTilt");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && !playerMovement.isInAir)
        {
            print("FTilt");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && playerMovement.isInAir)
        {
            isAttacking = true;
            print("Nair");
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") > 0 && playerMovement.isInAir)
        {
            print("Uair");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Vertical") < 0 && playerMovement.isInAir)
        {
            print("Dair");
            isAttacking = true;
        }

        else if (Input.GetButtonDown("Fire2") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0 && playerMovement.isInAir)
        {
            print("Fair");
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire1") && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f && !playerMovement.isInAir)
        {
            print("NSpecial");
            isSpecial = true;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") < 0f)
        {
            isAttacking = true;
            isSpecial = true;
            print("DSpecial");
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") > 0f)
        {
            isAttacking = true;
            isSpecial = true;
            print("USpecial");
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") == 0)
        {
            isAttacking = true;
            isSpecial = true;

            if (playerMovement.isInAir)
        {
            Physics2D.gravity = new Vector2(0, 0);
        }

            print("FSpecial");
        }



        else if (Input.GetButton("Fire3") && !strongDone && Input.GetAxisRaw("Vertical")==0 && !playerMovement.isInAir)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("FStrong Startup");
            }
            strongTimer += Time.deltaTime;

            if(strongTimer < 1.2f)
            {
                strongDamage += .001f;
                print(strongDamage);
            }

            if (strongTimer >= 2.2f)
            {
                strongDone = true;
            }

        }

        else if (Input.GetButton("Fire3") && !strongDone && Input.GetAxisRaw("Vertical") > 0 && !playerMovement.isInAir)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("UStrong Startup");
            }
            strongTimer += Time.deltaTime;

            if (strongTimer < 1.2f)
            {
                strongDamage += .001f;
                print(strongDamage);
            }

            if (strongTimer >= 2.2f)
            {
                strongDone = true;
            }

        }

        else if (Input.GetButton("Fire3") && !strongDone && Input.GetAxisRaw("Vertical") < 0 && !playerMovement.isInAir)
        {
            isAttacking = true;
            if (!strongStarted)
            {
                strongStarted = true;
                anim.SetBool("isStrong", strongStarted);
                anim.Play("DStrong Startup");
            }
            strongTimer += Time.deltaTime;

            if (strongTimer < 1.2f)
            {
                strongDamage += .001f;
                print(strongTimer);
            }

            if (strongTimer >= 2.2f)
            {
                strongDone = true;
            }

        }


        else if (Input.GetButtonUp("Fire3") || strongDone)
        {
            print("Releasing strong");
            strongStarted = false;
            anim.SetBool("isStrong", strongStarted);
            print(strongDamage);
            strongDamage = 0;
            strongTimer = 0;
            if (Input.GetButtonUp("Fire3"))
            {
                strongDone = false;
            }
        }


    }


    public void Jab1(int L)
    {
        hitBoxes[0].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Jab2(int L)
    {
        hitBoxes[1].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun 
    }

    public void Jab3(int L)
    {
        hitBoxes[2].SetActive(true);
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
        float XDir = float.Parse(statSplit[5]); //XDir
        float yDir = float.Parse(statSplit[6]); //YDir


        playerMovement.rb.AddForce(new Vector2(playerMovement.GetIsLeft() ? -XDir : XDir, yDir));
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

    public void FStrong(int L)
    {
        hitBoxes[13].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1])*strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]) + (float.Parse(statSplit[3]) * strongDamage); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        strongDamage = 0.0f;
    }

    public void UStrong(int L)
    {
        hitBoxes[14].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1])*strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[2]); //Angle
        currentStats[2] = float.Parse(statSplit[3]) + (float.Parse(statSplit[3]) * strongDamage); //Knockback
        currentStats[3] = float.Parse(statSplit[4]); //Time Stun
        strongDamage = 0.0f;
    }

    public void DStrong(int L)
    {
        hitBoxes[15].SetActive(true);
        string[] statSplit = Line[L].Split(" ");
        currentStats[0] = float.Parse(statSplit[1]) + (float.Parse(statSplit[1])*strongDamage); //Damage
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

        if (playerMovement.isInAir)
        {
            Physics2D.gravity = new Vector2(0, 0);
        }

        playerMovement.rb.AddForce(new Vector2( XDir, yDir));
    }




    void DespawnHitBox(int hitboxIndex)
    {
    if (hitboxIndex==16)
        {
           isSpecial = false;
        }

        hitBoxes[hitboxIndex].SetActive(false);
    }

    void DespawnStickyHand()
    {
        stickyHand.SetActive(false);
    }

    public void SetStrongDone(bool newStrongDone)
    {
        strongDone = newStrongDone;
    }

    public float[] GetCurrentStats()
    {
        return currentStats;
    }
}
