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
                    print(attackDetails[(int)attack]);
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

}
