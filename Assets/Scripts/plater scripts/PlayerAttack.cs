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
        Attack();
    }

    void Attack()
    {


        //Jab if statement
        if(Input.GetKeyDown(KeyCode.P) && !isAttacking && anim.GetBool("Idle")==true && !playerMovement.isInAir)
        {
            print("Jab");
            isSpecial = false;
            isAttacking = true;
        }

        else if(Input.GetKeyDown(KeyCode.O) && !isAttacking && anim.GetBool("Idle")==true && !playerMovement.isInAir)
        {
            print("Neutral B");
            isSpecial = true;
            isAttacking = true;
        }

        else if(Input.GetKeyDown(KeyCode.P) && !isAttacking && playerMovement.isInAir && !isAttacking && anim.GetBool("Idle") == false)
        {
            isAttacking = true;
            print("Nair");
        }

        else if(Input.GetKeyDown(KeyCode.O) && !isAttacking && Input.GetAxisRaw("Vertical") < 0f && !playerMovement.isInAir)
        {
            isAttacking = true;
            print("DownB");
        }

        else if (Input.GetKeyDown(KeyCode.O) && !isAttacking && Input.GetAxisRaw("Horizontal") != 0f && !playerMovement.isInAir)
        {
            isAttacking = true;
            print("Side B");
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

    void DespawnHitBox(int hitboxIndex)
    {
        hitBoxes[hitboxIndex].SetActive(false);
    }

    public float[] GetCurrentStats()
    {
        return currentStats;
    }
}
