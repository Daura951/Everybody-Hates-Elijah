using System.Collections;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField]
    private GameObject marker2D;
    [SerializeField]
    private Rigidbody2D rigidBody2D;

    private Vector2 velocity = new Vector2(1f, 0f); 
    private int direction = 1;

    private Vector3 home;
    private Vector3 end;

    void Start()
    {
        home = transform.position;
        end = marker2D.gameObject.transform.position;
    }

    private void FixedUpdate()
    {

        if (transform.position.x < end.x || transform.position.x > home.x)
        {
            direction *= -1;
        }

        Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

        rigidBody2D.MovePosition(pos + velocity * direction * Time.fixedDeltaTime);
    }
}