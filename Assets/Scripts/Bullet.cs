using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed , range , dir;
    private Vector3 start, End;
    // Start is called before the first frame update
    void Start()
    {
        start = this.transform.position;

        if (dir == 0)
            End = new Vector3(transform.position.x - range, transform.position.y, transform.position.z);
        else
            End = new Vector3(transform.position.x + range, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed);

         if ((dir == 0 && transform.position.x < End.x) || (dir != 0 && transform.position.x > End.x))
          {
              Destroy(this.gameObject);
          }

    }

    public void SetBull(float s , float r , float d)
    {
        speed = s;
        range = r;
        dir = d;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

}
