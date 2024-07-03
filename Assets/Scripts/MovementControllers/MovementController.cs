using UnityEngine;

public abstract class MovementController : MonoBehaviour 
{
    public abstract void MoveUp();

    public abstract void MoveDown();

    public abstract void MoveLeft();

    public abstract void MoveRight();
}