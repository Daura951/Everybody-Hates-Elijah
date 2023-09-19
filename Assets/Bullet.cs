using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeOnScreen = 5.0f;
    public float damage = 1.0f;
    public float speed = .1f;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed);

        if (timer >= timeOnScreen)
        {
            Destroy(this);
        }
        else timer += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            collision.gameObject.GetComponent<PlayerMovement>().ChangeHealth();
            Destroy(this.gameObject);
        }
    }
}
