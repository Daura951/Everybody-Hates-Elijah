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
        if(Input.GetKeyDown(KeyCode.P) && !isAttacking)
        {
            isAttacking = true;
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

    void DespawnHitBox(int hitboxIndex)
    {
        hitBoxes[hitboxIndex].SetActive(false);
    }

    public float[] GetCurrentStats()
    {
        return currentStats;
    }
}
