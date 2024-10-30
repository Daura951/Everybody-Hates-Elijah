
/*

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

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformMovement : MonoBehaviour
{
    private Vector3 home, End;
    public Vector3 EndSpot;
    public float speed;
    private bool bounce = true;
    GameObject player;
    private float startSpeed;
    private float halfSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        home = this.transform.position;
        End = this.transform.position + EndSpot;
        startSpeed = speed;
        halfSpeed = speed / 2;
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        if (bounce)
        {
            transform.position = Vector2.MoveTowards(transform.position, End, step);
            if (transform.position == End)
                bounce = !bounce;
        }
        if (!bounce)
        {
            transform.position = Vector2.MoveTowards(transform.position, home, step);
            if (transform.position == home)
                bounce = !bounce;
        }
    }
}