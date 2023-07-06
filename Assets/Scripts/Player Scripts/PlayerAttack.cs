using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public GameObject[] hitBoxes;

    private float[] currentStats;
    private void Start()
    {
        currentStats = new float[3];
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("FTilt");
        }

    }

    private void FTilt(string stats)
    {
        hitBoxes[0].SetActive(true);
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
