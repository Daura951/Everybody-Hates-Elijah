using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    private GameObject obj;
    private Rigidbody2D body;
    private float dir;
    public float velocity;

    void Start()
    {
         
    }

     private void OnCollisionEnter2D(Collision2D collision)
     {
            obj = collision.gameObject;
            body = obj.GetComponent<Rigidbody2D>();
            dir = obj.transform.rotation.y;
      }
}
