using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public Vector2 Input { get; private set; }

    public event Action<float> OnMove;
    public event Action OnJabPressed;
    public event Action OnTiltPressed;
    public event Action OnStrongHold;
    public event Action OnStrongRelease;
    public event Action OnSpecialPressed;

    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction neutral;
    private InputAction strong;
    public InputAction special;
    private InputAction jump;
    private InputAction run;


    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }
    public bool isNeutralPressed { get; private set; }
    public bool isTiltPressed { get; private set; }
    public bool isStrongHeld { get; private set;}
    public bool isJumping { get;  set; }
    public bool isGrounded { get; set; }

    public bool isRunning { get; private set; }

    private float tiltThreshold = 0.1f;    
    private float smashThreshold = 0.8f;  

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        neutral = playerControls.Player.Neutral;
        jump = playerControls.Player.Jump;
        run = playerControls.Player.Run;
        strong = playerControls.Player.Strong;
        special = playerControls.Player.Special;
        move.Enable();
        neutral.Enable();
        jump.Enable();
        run.Enable();
        strong.Enable();
        special.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        neutral.Disable();
        jump.Disable();
        run.Disable();
        strong.Disable();
        special.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerControls.Player.Strong.performed += OnStrongPerformed;
        playerControls.Player.Strong.canceled += OnStrongCanceled;
    }

    private void OnStrongPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Strong performed (held)");
        isStrongHeld = true;
        OnStrongHold?.Invoke();
    }

    private void OnStrongCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Strong canceled (released)");
        isStrongHeld = false;
        OnStrongRelease?.Invoke();
    }

    void Update()
    {
        JumpPressed = jump.WasPressedThisFrame();
        JumpReleased = jump.WasReleasedThisFrame();
        JumpHeld = jump.IsPressed();
        Input = move.ReadValue<Vector2>();

        moveHorizontal = move.ReadValue<Vector2>().x;
        moveVertical = move.ReadValue<Vector2>().y;
        OnMove?.Invoke((moveHorizontal > .1f || moveHorizontal < -.1f) ? moveHorizontal: 0.0f);
        isNeutralPressed = neutral.triggered;
        isRunning = run.ReadValue<float>() > 0;

        if (neutral.triggered && isGrounded)
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

        if(special.triggered)
        {
            OnSpecialPressed?.Invoke();
        }
    }
}
