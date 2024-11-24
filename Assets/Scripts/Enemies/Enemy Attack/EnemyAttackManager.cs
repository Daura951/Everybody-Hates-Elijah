using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    private int currentHitboxIndex = -1;
    private AttackDetails[] enemyAttackDetails;
    public GameObject[] hitboxes;

    public EnemyHurtVals.AttackTypes[] attacksEnemyHas;

    private void Start()
    {
        enemyAttackDetails = new AttackDetails[attacksEnemyHas.Length];
        foreach (GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }

        for (int i = 0; i < attacksEnemyHas.Length; i++)
        {
            enemyAttackDetails[i] = EnemyHurtVals.convertDictValToAttackDetails(attacksEnemyHas[i]);
        } 

    }

    public void SpawnHitbox(int attackIndex)
    {
        foreach (GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }

        currentHitboxIndex = attackIndex;
        hitboxes[currentHitboxIndex].SetActive(true);
    }

    public void DespawnHitbox()
    {
        foreach (GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }
        if (currentHitboxIndex >= 0)
        {
            hitboxes[currentHitboxIndex].SetActive(false);
            currentHitboxIndex = -1;
        }
    }
}
