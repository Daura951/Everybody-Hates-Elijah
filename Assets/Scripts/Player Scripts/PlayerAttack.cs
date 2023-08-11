using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Awake()
    {
        stickyHand.SetActive(false);
        attackInstance = this;
    }

    private void Start()
    {
        currentStats = new float[3];
        anim = GetComponent<Animator>();
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


        else if (Input.GetButtonDown("Fire1") && !isAttacking && anim.GetBool("Idle") == true && !playerMovement.isInAir && Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f)
        {
            print("NSpecial");
            isSpecial = true;
            isAttacking = true;
        }


        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") < 0f && !playerMovement.isInAir)
        {
            isAttacking = true;
            isSpecial = true;
            print("DSpecial");
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Vertical") > 0f && !playerMovement.isInAir)
        {
            isAttacking = true;
            isSpecial = true;
            print("USpecial");
        }

        else if (Input.GetButtonDown("Fire1") && !isAttacking && Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") == 0 && !playerMovement.isInAir)
        {
            isAttacking = true;
            isSpecial = true;
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
            strongTimer = 0;
            if (Input.GetButtonUp("Fire3"))
            {
                strongDone = false;
            }
        }


    }


    public void Jab1(string stats)
    {
        hitBoxes[0].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Jab2(string stats)
    {
        hitBoxes[1].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Jab3(string stats)
    {
        hitBoxes[2].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Nair(string stats)
    {
        hitBoxes[3].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }
    public void DownB(string stats)
    {
        hitBoxes[4].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void SideB(string stats)
    {
        hitBoxes[5].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
        float XDir = float.Parse(statSplit[3]); //XDir
        float yDir = float.Parse(statSplit[4]); //YDir


        playerMovement.rb.AddForce(new Vector2(playerMovement.GetIsLeft() ? -XDir : XDir, yDir));
    }

    public void NeutralB(string stats)
    {
        hitBoxes[6].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }


    public void UTilt(string stats)
    {
        hitBoxes[7].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void FTilt(string stats)
    {
        hitBoxes[8].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void DTilt(string stats)
    {
        hitBoxes[9].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Fair(string stats)
    {
        hitBoxes[10].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Uair(string stats)
    {
        hitBoxes[11].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void Dair(string stats)
    {
        hitBoxes[12].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
    }

    public void FStrong(string stats)
    {
        hitBoxes[13].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]) + (float.Parse(statSplit[0])*strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]) + (float.Parse(statSplit[2]) * strongDamage); //Knockback
        strongDamage = 0.0f;
    }

    public void UStrong(string stats)
    {
        hitBoxes[14].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]) + (float.Parse(statSplit[0]) * strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]) + (float.Parse(statSplit[2]) * strongDamage); //Knockback
        strongDamage = 0.0f;
    }

    public void DStrong(string stats)
    {
        hitBoxes[15].SetActive(true);
        string[] statSplit = stats.Split(" ");
        currentStats[0] = float.Parse(statSplit[0]) + (float.Parse(statSplit[0]) * strongDamage); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]) + (float.Parse(statSplit[2]) * strongDamage); //Knockback
        strongDamage = 0.0f;
    }

    public void USpecial(string stats)
    {
        string[] statSplit = stats.Split(" ");
        hitBoxes[int.Parse(statSplit[5])].SetActive(true);
        currentStats[0] = float.Parse(statSplit[0]); //Damage
        currentStats[1] = float.Parse(statSplit[1]); //Angle
        currentStats[2] = float.Parse(statSplit[2]); //Knockback
        float XDir = float.Parse(statSplit[3]); //XDir
        float yDir = float.Parse(statSplit[4]); //YDir

        playerMovement.rb.AddForce(new Vector2( XDir, yDir));
    }




    void DespawnHitBox(int hitboxIndex)
    {
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