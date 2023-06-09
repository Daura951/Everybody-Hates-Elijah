using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isHit = false, isLeft = false;
    private float[] stats;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            GetHit(stats[2], stats[1]);
        }
    }

    private void GetHit(float knockBack, float angle)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * knockBack;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * knockBack;

        if(isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent));

        isHit = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Hitbox")
        {
            print("WHat?");
            stats = collision.transform.parent.gameObject.GetComponent<PlayerAttack>().GetCurrentStats();
            isHit = true;
            isLeft = collision.transform.parent.gameObject.GetComponent<PlayerMovement>().GetIsLeft();
        }
    }
}
