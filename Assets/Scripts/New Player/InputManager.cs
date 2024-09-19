using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager:PlayerComponent
{

    public event Action<float> OnMove;
    public event Action OnJabPressed;

    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction neutral;
    private InputAction jump;


    public float moveHorizontal { get; private set; }
    public bool isNeturalPressed { get; private set; }
    public bool isJumping { get; private set; }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        neutral = playerControls.Player.Neutral;
        jump = playerControls.Player.Jump;
        move.Enable();
        neutral.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        neutral.Disable();
        jump.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerControls.Player.Jump.performed += JumpOn;
        playerControls.Player.Jump.canceled += JumpOff;
    }

    private void JumpOn(InputAction.CallbackContext obj)
    {
        isJumping = true;
    }

    private void JumpOff(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        moveHorizontal = playerControls.Player.Move.ReadValue<Vector2>().x;
        OnMove?.Invoke((moveHorizontal > .1f || moveHorizontal < -.1f) ? moveHorizontal: 0.0f);

        isNeturalPressed = playerControls.Player.Neutral.triggered;

        if(playerControls.Player.Neutral.triggered)
        {
            OnJabPressed?.Invoke();
        }
    }

    public override void OnFixedUpdate()
    {

    }
}
