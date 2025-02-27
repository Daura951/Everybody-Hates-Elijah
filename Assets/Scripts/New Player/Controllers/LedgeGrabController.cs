using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabController : PlayerComponent
{

    public bool Draw;

    private PlayerMovementController PMC;

    private float Xpos, Ypos;

    public bool grab = false, moveable = false , moveHorOnLedge = false;

    /// <summary>
    /// Range of the Detection Box for Ledge Grabbing
    /// <param name="XOffset">Animation to play</param>
    /// </summary>


    public float XOffset, YOffset, XSize, YSize;
    public LayerMask groundMask;
    [Range(-4, 4)] public float Xtweak = 0, Ytweak = 0;
    private GameObject g;

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
        if (!playerState.isLedgeGrab)
            LedgeCheck();
    }

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);
        inputManager.OnLedgeInput += LedgeHang;
    }



    private void LedgeCheck()
    {
        XOffset = playerState.isRight ? MathF.Abs(XOffset) : MathF.Abs(XOffset) * -1f;


        Collider2D redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + XOffset, transform.position.y + YOffset), new Vector2(XSize, YSize), 0f, groundMask);

        if (!redBox || animController.GetCurrentAnimation() == Animations.FALLING)
        {
            playerState.isLedgeGrab = false;
        }

        if (redBox && playerState.CanLedgeGrab)
        {
            // print("I grabbed because why not");
            g = redBox.gameObject;

            if (g.GetComponent<PlatformMovement>())
                moveable = true;

            if (g.CompareTag("Grab") && playerState.CanLedgeGrab)
            {
                playerState.isLedgeGrab = true;
                playerState.CanLedgeGrab = false;
                
                playerState.CanUSpecial = true;
                playerState.CanSSpecial = true;
                playerState.isSSpecial = false;
                playerState.isUSpecial = false;
                moveHorOnLedge = false;

                BoxCollider2D Box = g.GetComponent<BoxCollider2D>();
                PMC.LedgeGrabStop();

                animController.StopAllCoroutines();


                Xpos = g.transform.position.x + ((transform.position.x < g.transform.position.x ? -1f : 1f) * ((Box.size.x * .5f * g.transform.localScale.x) + .55f));
                Ypos = g.transform.position.y + ((Box.size.y * .5f + Box.offset.y) * g.transform.localScale.y) - .5f;
                transform.position = new Vector2(Xpos, Ypos);

                if (transform.position.x < g.transform.position.x)
                {

                    if (playerState.isRight)
                        PMC.Turn(false);
                }
                else
                {
                    if (!playerState.isRight)
                        PMC.Turn(true);

                }
                animController.Play(Animations.LEDGE_GRAB, true, true);
            }
        }
    }

    public void LedgeMove()
    {
        float newY = Ypos + Ytweak;
        float newX = transform.position.x + (((transform.position.x < g.transform.position.x) ? 1f : -1f) * Xtweak);
        transform.position = new Vector2(newX, newY);
    }

    public void LedgeRotate()
    {
        if (transform.position.x < g.transform.position.x)
        {
            if (!playerState.isRight)
                PMC.Turn(true);

        }
        else
        {
            if (playerState.isRight)
                PMC.Turn(false);
        }
    }

    public void LedgeHang(float moveX, float moveY)
    {
        if (playerState.isLedgeGrab)
        {
            if (moveX > -.1f && moveX < .1f)
                moveHorOnLedge = true;

            if (moveY < -.4f)
            {
                print("Fall input");
                animController.Play(Animations.FALLING, false, true);
                playerState.isLedgeGrab = false;
            }

            if (moveHorOnLedge && ((!playerState.isRight && moveX > .1f) || (playerState.isRight && moveX < -.1f)))
            {
                animController.Play(Animations.LEDGE_PULL, false, false);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if(Draw)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector2(transform.position.x + XOffset, transform.position.y + YOffset), new Vector2(XSize, YSize));
        }
    }

}
