using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Class that houses utility methods for attacks. Accessed via the animator
/// </summary>
public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hitboxes;

    private AttackDetails[] attackDetails;

    [SerializeField]
    private TextAsset attackConfigFile;

    private int currentHitboxIndex = -1;

    private PlayerState playerState;

    public bool didStickyCollide = false;

    public Stickyhand stickyhand;

    void Start()
    {
        attackDetails = new AttackDetails[hitboxes.Length];
        InitalizeManager();
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
                    float freezeDuration = float.Parse(parts[5]);

                    attackDetails[(int)attack] = new AttackDetails(attack, damage, angle, knockback, stunTime, freezeDuration);
                    //print(attackDetails[(int)attack] + " " + hitboxes[(int)attack]);
                }
            }
        }
    }

    /// <summary>
    /// Spawns a hitbox correlating to the attack
    /// </summary>
    /// <param name="attack">Attack the player is performing</param>
    public void SpawnHitbox(Attacks attack)
    {
        foreach(GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }

        currentHitboxIndex = (int)attack;
        hitboxes[currentHitboxIndex].SetActive(true);
    }

    /// <summary>
    /// Despawns the currently active hitbox so long as its index is not negative
    /// </summary>
    public void DespawnHitbox()
    {
        foreach (GameObject hb in hitboxes)
        {
            hb.SetActive(false);
        }
        currentHitboxIndex = -1;
        
    }

    /// <summary>
    /// Finds PlayerAttackDetails via the hitbox's name
    /// </summary>
    /// <param name="hbName">Name of the hitbox</param>
    /// <returns></returns>
    public AttackDetails findByHitbox(string hbName)
    {
        for(int i = 0; i < hitboxes.Length; i++)
        {
            if (hitboxes[i].name == hbName)
            {
                //print(attackDetails[i]);
                return attackDetails[i];
            }
        }
        return null;
    }

    public PlayerState GetPlayerState()
    {
        return playerState;
    }
    #region StickyHand

    public void StickyHandStartup()
    {
        stickyhand.SetIsStickyActive(true);
        stickyhand.ChangeStickyHandAnimation("Stickyhand Start up");
    }

    public void StickyHandSuccess()
    {
        stickyhand.ChangeStickyHandAnimation("Stickyhand Success");
        this.didStickyCollide = false;
    }

    public void StickyHandFail()
    {
        stickyhand.ChangeStickyHandAnimation("Stickyhand Fail");
    }

    public void OnStickyHandEnd()
    {
        stickyhand.ChangeStickyHandAnimation("Stickyhand Blank State");
        stickyhand.SetIsStickyActive(false);
    }
    #endregion

    #region Specials

    public void USpecialStart()
    {
        playerState.isUSpecial = true;
    }

    public void USpecialEnd()
    {
        playerState.isUSpecial = false;
        playerState.isHelpless = true;
    }

    public void SSpecialStart()
    {
        playerState.isSSpecial = true;
    }

    public void SSpecialEnd()
    {
        playerState.isSSpecial = false;
        playerState.isHelpless = true;
    }

    public void DSpecialStart()
    {
        playerState.isDSpecial = true;
    }

    public void DSpecialEnd()
    {
        playerState.isDSpecial = false;
        playerState.isHelpless = true;
    }


    #endregion


    public void OnLedgeFail()
    {
        playerState.isLedgeGrab = false;
    }
}
