using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escelator : MonoBehaviour
{
    PlayerMovement PM;
    Rigidbody2D rb;
    GameObject player;
    private float angle , X , Y;
    public float speed = 1;
    private bool InAir , OnEscelator;


    // Start is called before the first frame update
    void Start()
    {
      player = GameObject.FindGameObjectWithTag("Player");
      PM = player.GetComponent<PlayerMovement>();
      rb = player.GetComponent<Rigidbody2D>();
      angle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
       InAir = PM.GetIsInAir();
       
       if(OnEscelator)
       {
         X = Mathf.Cos(angle * (Mathf.PI / 180)) * Time.deltaTime * speed;
         Y = Mathf.Sin(angle * (Mathf.PI / 180)) * Time.deltaTime * speed;
         
        player.transform.position += new Vector3(X , Y , 0);
       }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject == player)
        {
            OnEscelator = true;
            rb.velocity = new Vector2(0,0);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject == player)
        {
            OnEscelator = false;
            if(!Input.GetKey(KeyCode.Space))
            {
            rb.velocity = new Vector2(0,0);
            }
        }
    }

        public bool GetOnEscelator()
    {
        return OnEscelator;
    }

}
