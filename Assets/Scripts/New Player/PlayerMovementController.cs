using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

    [Header("Refrences")]
    public PlayerMovementStats pms;
    [SerializeField] private Collider2D feet;
    [SerializeField] private Collider2D body;

    private Rigidbody2D rb;

    // move vars
    private Vector2 moveVelocity;
    private bool isRight;

    //collision checks vars
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool grounded;
    private bool bumpedHead;


    // idk
    private float accel;
    public float decel;

    public override void OnStart()
    {
        print("Hello PlayerMovementController!");
        rb = GetComponent<Rigidbody2D>();
        isRight = true;
    }

    public override void OnUpdate()
    {
        if (!playerState.isAttacking)
        {

            if (inputManager.isJumping)
            {
                print("Jump");
            }
        }
    }
    public override void OnFixedUpdate()
    {
    }


    public void MovePlayer(float moveX)
    {
        if (!playerState.isAttacking)
        {
            if (moveX != 0)
            {
                //move check
                TurnCheck(moveX);

                if (inputManager.isRunning)
                animController.Play(Animations.RUN, false, false);
                else
                    animController.Play(Animations.WALK, false, false);


                Vector2 targetVelocity = Vector2.zero;
                targetVelocity = new Vector2(moveX, 0f) * (inputManager.isRunning ? pms.MaxRunSpeed : pms.MaxWalkSpeed);
                moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, accel * Time.fixedDeltaTime);
                rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);

            }
            else animController.Play(Animations.IDLE, false, false);
            // transform.Translate(new Vector2(moveX / 100.0f, 0.0f));

            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);

        }
    }

    private void TurnCheck(float moveX)
    {
        if (isRight && moveX < 0)
            Turn(false);
        if (!isRight && moveX > 0)
            Turn(true);
    }

    private void Turn(bool turn)
    {
        isRight = turn;
        transform.Rotate(0f,180f *(turn ? 1f : -1f) ,0f);
    }


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;


        inputManager.OnMove+=MovePlayer;
    }
}
