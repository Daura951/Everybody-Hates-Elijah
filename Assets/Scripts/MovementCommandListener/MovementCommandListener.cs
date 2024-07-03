using UnityEngine;

public class MovementCommandListener : MonoBehaviour
{
    [SerializeField]
    private MovementController m_MovementController;

    private void OnEnable()
    {
        InputManager.onMoveRight += (CommandMoveRight m) => moveRight(m);
        InputManager.onMoveLeft += (CommandMoveLeft m) => moveLeft(m);
        InputManager.onMoveUp += (CommandMoveUp m) => moveUp(m);
        InputManager.onMoveDown += (CommandMoveDown m) => moveDown(m);
    }

    private void moveRight(CommandMoveRight m)
    {
        m.SetController(m_MovementController);
        m.Execute();
    }
    private void moveLeft(CommandMoveLeft m)
    {
        m.SetController(m_MovementController);
        m.Execute();
    }
    private void moveUp(CommandMoveUp m)
    {
        m.SetController(m_MovementController);
        m.Execute();
    }
    private void moveDown(CommandMoveDown m)
    {
        m.SetController(m_MovementController);
        m.Execute();
    }
    private void OnDisable()
    {
        InputManager.onMoveRight -= (CommandMoveRight m) => moveRight(m);
        InputManager.onMoveLeft -= (CommandMoveLeft m) => moveLeft(m);
        InputManager.onMoveUp -= (CommandMoveUp m) => moveUp(m);
        InputManager.onMoveDown -= (CommandMoveDown m) => moveDown(m);
    }
}