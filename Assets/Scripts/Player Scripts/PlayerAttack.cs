using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public bool isAttacking = false;
    public static PlayerAttack attackInstance;
    public GameObject[] hitBoxes;
    private float[] currentStats;
    public PlayerMovement playerMovement;

    private void Awake()
    {
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
        if(Input.GetKeyDown(KeyCode.P) && !isAttacking && anim.GetBool("Idle")==true && !playerMovement.GetIsInAir())
        {
            print("Jab");
            isAttacking = true;
        }

        else if(Input.GetKeyDown(KeyCode.P) && !isAttacking && playerMovement.GetIsInAir() && !isAttacking && anim.GetBool("Idle") == false)
        {
            isAttacking = true;
            print("Nair");
        }

        else if(Input.GetKeyDown(KeyCode.O) && !isAttacking && Input.GetAxisRaw("Vertical") < 0f && !playerMovement.GetIsInAir())
        {
            isAttacking = true;
            print("DownB");
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

    void DespawnHitBox(int hitboxIndex)
    {
        hitBoxes[hitboxIndex].SetActive(false);
    }

    public float[] GetCurrentStats()
    {
        return currentStats;
    }
}
