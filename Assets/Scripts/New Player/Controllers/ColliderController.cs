using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ColliderController : PlayerComponent
{
    public bool Draw;

    [SerializeField] private Collider2D feet;
    [SerializeField] private Collider2D body;

    private RaycastHit2D groundHit;
    private PlayerMovementController PMC;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectLength = 0.02f;
    public float HeadDetectLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = .75f;

    public override void OnStart()
    {
        // This is how the generics of something :)
        PMC = player.GetController<PlayerMovementController>();
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        IsGrounded();
        bumpHead();
    }

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);
    }

    #region Player Collision Check
    private void IsGrounded()
    {
        if (playerState.isGrounded)
        {
            playerState.CanUSpecial = playerState.CanUSpecial ? playerState.CanUSpecial : true;
            playerState.CanSSpecial = playerState.CanSSpecial ? playerState.CanSSpecial : true;
            playerState.isHelpless = playerState.isHelpless ? false : playerState.isHelpless;
            PMC.SSpecialFall = PMC.SSpecialFall ? false : PMC.SSpecialFall;
        }
        else
        {
            playerState.CanUSpecial = playerState.isUSpecial ? false : playerState.CanUSpecial;
            playerState.CanSSpecial = playerState.isSSpecial ? false : playerState.CanSSpecial;
        }

        Vector2 boxCastOrigin = new Vector2(feet.bounds.center.x, feet.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feet.bounds.size.x, GroundDetectLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, GroundDetectLength, GroundLayer);
        playerState.isGrounded = groundHit.collider != null ? true : false;

        if (Draw)
        {
            Color rayColor;
            rayColor = playerState.isGrounded ? Color.green : Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * GroundDetectLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * GroundDetectLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - GroundDetectLength), Vector2.right * boxCastSize.x, rayColor);

        }
    }

    private void bumpHead()
    {
        Vector2 boxCastOrigin = new Vector2(feet.bounds.center.x, body.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feet.bounds.size.x * HeadWidth, HeadDetectLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, HeadDetectLength, GroundLayer);
        PMC.bumpedHead = groundHit.collider != null ? (groundHit.collider.GetComponent<PlatformEffector2D>() == true ? false : true) : false;
    }
    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedGO = collision.gameObject;

        if (collidedGO.GetComponent<PlatformMovement>() != null && !playerState.isOnMoveable)
        {
            playerState.isOnMoveable = true;
            transform.SetParent(collision.gameObject.transform);
            PMC.Interpolate(false);
        }

        if (collidedGO.GetComponent<Stun_Info>() == null)
        {
            //if (playerState.VelocityStunned) { EventManager.TriggerOnStunEnd(); }
            playerState.VelocityStunned = false;
        }
        else
        {
            if (playerState.isStunned) { EventManager.TriggerOnStunEnd(); }

            ApplyStunVales();

            AttackDetails envAttack = EnvironmentHurtVals.convertDictValToAttackDetails(collidedGO.GetComponent<EnvironmentalStun_info>().attackType);
            print("STUN HAZARD!");
            EventManager.TriggerOnStunStart();
            PerformHazardCollision(envAttack, collision.contacts[0].normal);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformMovement>() != null && playerState.isOnMoveable)
        {
            playerState.isOnMoveable = false;
            transform.SetParent(null);
            PMC.Interpolate(true);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedGo = collision.gameObject;
        if (collision.gameObject.tag == "EHitbox")
        {
            //if (playerState.isStunned)  { EventManager.TriggerOnStunEnd(); }

            ApplyStunVales();

            AttackDetails hit = collidedGo.GetComponentInParent<EnemyAttackManager>().findByHitbox(collidedGo.name);
            EventManager.TriggerOnStunStart();
            PMC.ApplyKnockback(hit.Angle, hit.Knockback);
        }
    }

    private void PerformHazardCollision(AttackDetails attackDetails, Vector2 normal)
    {
        normal = normal.normalized;
        float knockbackAngle = (Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg);
        if (Mathf.Abs(normal.y) > 0.9f)
        {

            knockbackAngle = (normal.y > 0) ? 90f : -90f;
        }
        else
        {
            knockbackAngle = (normal.x > 0) ? 45f : -45f;
        }


        //print(normal + " " + knockbackAngle);
        player.GetController<PlayerMovementController>().ApplyKnockback(knockbackAngle, attackDetails.Knockback);
        player.GetController<PlayerMovementController>().usedJumps = 1;
    }

    private void ApplyStunVales()
    {
        playerState.isHelpless = false;
        playerState.isStunned = true;
        playerState.VelocityStunned = true;
    }

}
