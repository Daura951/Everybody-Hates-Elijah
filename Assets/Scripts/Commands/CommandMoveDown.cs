using UnityEngine;

public class CommandMoveDown : Command
{
    private MovementController _controller;

    public void SetController(MovementController controller)
    {
        _controller = controller;
    }

    public override void Execute()
    {
        if (_controller == null) return;
        _controller?.MoveDown();
    }
}