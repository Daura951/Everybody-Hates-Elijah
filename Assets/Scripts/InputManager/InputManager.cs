using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float deadZone = 0.2f;

    public delegate void OnMoveRight(CommandMoveRight commandMoveRight);
    public static OnMoveRight onMoveRight;
    public delegate void OnMoveLeft(CommandMoveLeft commandMoveLeft);
    public static OnMoveLeft onMoveLeft;
    public delegate void OnMoveUp(CommandMoveUp commandMoveUp);
    public static OnMoveUp onMoveUp;
    public delegate void OnMoveDown(CommandMoveDown commandMoveDown);
    public static OnMoveDown onMoveDown;



    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

            //Debug.Log("Horizontal: " + horizontal);
            //Debug.Log("Vertical: " + vertical);
        if (horizontal >= 0.5f + deadZone)
        {
            CommandMoveRight command_right = new CommandMoveRight();
            onMoveRight?.Invoke(command_right);
            //Debug.Log("--- Moving right");
        }
        if (horizontal <= -0.5f - deadZone)
        {
            CommandMoveLeft command_left = new CommandMoveLeft();
            onMoveLeft?.Invoke(command_left);
            //Debug.Log("--- Moving left");
        }
        if (vertical >= 0.5f + deadZone)
        {
            CommandMoveUp command_up = new CommandMoveUp();
            onMoveUp?.Invoke(command_up);
            //Debug.Log("--- Moving up");
        }
        if (vertical <= -0.5f - deadZone)
        {
            CommandMoveDown command_down = new CommandMoveDown();
            onMoveDown?.Invoke(command_down);
            //Debug.Log("--- Moving down");
        }
    }


}
    