using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool Stunned = false;
    public float lag;
    public string ObjTag;

    public bool isLeft;
    public float angle , knockBack;
    private Rigidbody2D rb;
    PlayerMovement PM;
    

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       PM = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {   
        isLeft = PM.GetIsLeft();
    }

    private void GetHit(float knockBack, float angle)
    {
        float XComponent = Mathf.Cos(angle * (Mathf.PI / 180)) * knockBack;
        float YComponent = Mathf.Sin(angle * (Mathf.PI / 180)) * knockBack;

        

        if(!isLeft)
        {
            XComponent *= -1;
        }

        rb.AddForce(new Vector2(XComponent, YComponent) , ForceMode2D.Impulse);
    }
               
   


    void OnTriggerEnter2D(Collider2D col)
    {
     if (col.gameObject.tag == ObjTag)
      {
      Stunned = true;
      rb.velocity = new Vector2(0,0);
      GetHit(knockBack , angle);
      StartCoroutine(StunLag());
      
      }
    }

    public bool getIsStunned()
    {
        return Stunned;
    }

    private IEnumerator StunLag()
    {
        yield return new WaitForSeconds(lag);
        Stunned = false;
    }
}
