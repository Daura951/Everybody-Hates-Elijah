using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public event Action<float> OnMove;
    public event Action OnJabPressed;
    public event Action OnTiltPressed;
    public event Action OnSmashPressed;

    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction neutral;
    private InputAction jump;
    private InputAction run;


    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }
    public bool isNeutralPressed { get; private set; }
    public bool isTiltPressed { get; private set; }
    public bool isSmashPressed { get; private set;}
    public bool isJumping { get; private set; }

    public bool isRunning { get; private set; }

    private float tiltThreshold = 0.1f;    
    private float smashThreshold = 0.8f;  

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        neutral = playerControls.Player.Neutral;
        jump = playerControls.Player.Jump;
        run = playerControls.Player.Run;
        move.Enable();
        neutral.Enable();
        jump.Enable();
        run.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        neutral.Disable();
        jump.Disable();
        run.Disable();
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

    void Update()
    {
        moveHorizontal = move.ReadValue<Vector2>().x;
        moveVertical = move.ReadValue<Vector2>().y;
        OnMove?.Invoke((moveHorizontal > .1f || moveHorizontal < -.1f) ? moveHorizontal: 0.0f);
        isNeutralPressed = neutral.triggered;
        isRunning = run.ReadValue<float>() > 0;

        if(neutral.triggered)
        {

            if (move.ReadValue<Vector2>().magnitude < tiltThreshold)
            {
                OnJabPressed?.Invoke();
            }
            else if (move.ReadValue<Vector2>().magnitude >= tiltThreshold)
            {
                OnTiltPressed?.Invoke(); 
            }
        }
    }
}
