using UnityEngine;

public class MovementControllerDynamicRigidBody2d : MovementController
{
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float upForce = 1f;
    [SerializeField]
    private float downForce = 1f;
    [SerializeField]
    private float leftForce = 1f;
    [SerializeField]
    private float rightForce = 1f;

    public override void MoveUp()
    {
        rb2d?.AddForce(transform.up * upForce);
    }
    public override void MoveDown()
    {
        rb2d?.AddForce(transform.up * -downForce);
    }
    public override void MoveLeft()
    {
        rb2d?.AddForce(transform.right * -leftForce);
    }
    public override void MoveRight()
    {
        rb2d?.AddForce(transform.right * rightForce);
    }
}