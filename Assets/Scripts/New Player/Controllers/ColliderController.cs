using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : PlayerComponent
{
    public bool Draw;

    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

    [SerializeField] private Collider2D feet;
    [SerializeField] private Collider2D body;

    private RaycastHit2D groundHit;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectLength = 0.02f;
    public float HeadDetectLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = .75f;

    public override void OnStart()
    {
        // This is how the generics of something :)
        //PMC = player.GetControllers<PlayerMovementController>();
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
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;
    }

    #region Player Collision Check
    private void IsGrounded()
    {
        if (playerState.isGrounded)
        {
            playerState.CanUSpecial = playerState.CanUSpecial ? playerState.CanUSpecial : true;
            playerState.CanSSpecial = playerState.CanSSpecial ? playerState.CanSSpecial : true;
            playerState.isHelpless = playerState.isHelpless ? false : playerState.isHelpless;
            //pms.SSpecialFall = pms.SSpecialFall ? false : pms.SSpecialFall;
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
        //bumpedHead = groundHit.collider != null ? (groundHit.collider.GetComponent<PlatformEffector2D>() == true ? false : true) : false;
    }
    #endregion

}
