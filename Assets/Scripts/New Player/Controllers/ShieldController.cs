using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class ShieldController : PlayerComponent
{
    public GameObject Shield;
    private Animator ShieldAnim;
    private float AVGShieldTime, StoredShieldTime, StoredStunTime;
    public float ShieldTimer, StunTimer, SL;
    private bool shieldbroke;

    [Header("Shield Fraction Damage")]
    public float lowfrac;
    public float medfrac;
    public float highfrac;

    [Header("Hit Threshold")]
    //This junk could be connected to health im to lazy so its also here
    public float lowLim;
    public float medLim;

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);
        inputManager.OnShieldHold += StartShielding;
        inputManager.OnShieldRelease += EndShielding;
    }

    public override void OnStart()
    {
        ShieldAnim = Shield.GetComponent<Animator>();
        StoredShieldTime = ShieldTimer - 1f;
        StoredStunTime = StunTimer - 1f;
        AVGShieldTime = (1f - SL) / ShieldTimer;
        ShieldTimer = 0;
    }

    public override void OnFixedUpdate()
    {
    
    }


    public override void OnUpdate()
    {
        if(!playerState.isShielding && playerState.CanShield)
            ShieldTimer = ShieldTimer <= 0? 0 : ShieldTimer - Time.deltaTime;


        //Player has ability to shield and not in a shield break
        if (playerState.isShielding && playerState.CanShield)
        {
            if (ShieldTimer < StoredShieldTime)
            {
                ShieldTimer += Time.deltaTime;
                float scale = 1f - (AVGShieldTime * ShieldTimer);
                Shield.transform.localScale = new Vector3(scale, scale, 1f);
            }
            else
                ShieldTimer = StoredShieldTime;
            if (ShieldTimer == StoredShieldTime)
            {
                StunTimer = 0;
                playerState.CanShield = false;
                StartCoroutine(ShieldingFail());
            }
        }
        // Player is in a broken shield state
        if (!playerState.CanShield)
        {
            if (StunTimer < StoredStunTime)
            {
                StunTimer += Time.deltaTime;
            }
            else
                StunTimer = StoredStunTime;
            if (StunTimer == StoredStunTime)
            {
                playerState.CanShield = true;
                EventManager.TriggerOnShieldBreakEnd();
                animController.Play(Animations.IDLE, false, true);
            }
        }

        SendDataToPlayerState();
    }

    private void SendDataToPlayerState()
    {

    }

    private void StartShielding()
    {
        playerState.isShielding = true;
        Shield.SetActive(true);
        animController.Play(Animations.Shield_Start, true, true);
    }

    private void EndShielding()
    {
        if(!shieldbroke)
        {
        print("Let go");
        Shield.SetActive(false);
        playerState.isShielding = false;
        animController.Play(Animations.IDLE, false, true);
        }
        else
            shieldbroke = false;
    }

    public IEnumerator ShieldingFail()
    {
        print("Failed");
        shieldbroke = true;
        EventManager.TriggerOnShieldBreakStart();
        animController.Play(Animations.Shield_Pop, false, false);
        playerState.isShielding = false;
        ShieldAnim.Play("ShieldBreak");
        yield return new WaitForSeconds(ShieldAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Shield.SetActive(false);
        animController.Play(Animations.Shield_Stun_Transition, false, true);
    }

    #region Shield Damage

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedGO = collision.gameObject;
        if (collidedGO.gameObject.tag == "EHitbox")
        {
            float hurt = collidedGO.GetComponentInParent<EnemyAttackManager>().findByHitbox(collidedGO.name).Damage;
            if (playerState.isShielding)
            {
                if (hurt < lowLim && playerState.Health >= playerState.MaxHealth * .5)
                    ShieldDamag(1);
                else if ((hurt < medLim && playerState.Health >= playerState.MaxHealth * .5) || (hurt < lowLim && playerState.Health < playerState.MaxHealth * .5))
                    ShieldDamag(2);
                else if ((hurt < medLim && playerState.Health <= playerState.MaxHealth * .5) || hurt >= medLim)
                    ShieldDamag(3);
            }

        }
    }


    public void ShieldDamag(int x)
    {
        switch (x)
        {
            case 1:
                ShieldTimer += StoredShieldTime * lowfrac;
                break;

            case 2:
                ShieldTimer += StoredShieldTime * medfrac;
                break;

            case 3:
                ShieldTimer += StoredShieldTime * highfrac;
                break;
        }
    }

    #endregion

}
