using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Rigidbody2D body;
    public float velocity;

    void Start()
    {
        body = player.GetComponent<Rigidbody2D>(); 
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
            if (collision.gameObject.tag == "Player")
            {
                body.AddForce(Vector2.up * 100 * velocity);
            }
      }
}
