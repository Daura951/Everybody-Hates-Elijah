using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hitboxes;

    private PlayerAttackDetails[] attackDetails;

    [SerializeField]
    private TextAsset attackConfigFile;

    private int currentHitboxIndex = -1;

    private PlayerState playerState;

    void Start()
    {
        attackDetails = new PlayerAttackDetails[hitboxes.Length];
        InitalizeManager();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitalizeManager()
    {
        playerState = GetComponent<PlayerState>();
        using(StringReader sr = new StringReader(attackConfigFile.text))
        {
            string line;

            while((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Trim().Split(" ");

                Attacks attack;

                if(Enum.TryParse(parts[0], out attack))
                {
                    float damage = float.Parse(parts[1]);
                    float angle = float.Parse(parts[2]);
                    float knockback = float.Parse(parts[3]);
                    float stunTime = float.Parse(parts[4]);

                    attackDetails[(int)attack] = new PlayerAttackDetails(attack, damage, angle, knockback, stunTime);
                }
            }
        }
    }

    public void SpawnHitbox(Attacks attack)
    {
        foreach(GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }

        currentHitboxIndex = (int)attack;
        hitboxes[currentHitboxIndex].SetActive(true);
    }

    public void DespawnHitbox()
    {
        hitboxes[currentHitboxIndex].SetActive(false);
        currentHitboxIndex = -1;
    }

    public PlayerAttackDetails findByHitbox(string hbName)
    {
        for(int i = 0; i < hitboxes.Length; i++)
        {
            if (hitboxes[i].name == hbName)
            {
                print(attackDetails[i]);
                return attackDetails[i];
            }
        }
        return null;
    }

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

}
