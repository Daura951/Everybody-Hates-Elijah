using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_UnhideObject : MonoBehaviour
{
     public float hide, appear;
    private bool hiding = false, seeing = false;
    private float counter = 0;
    private SpriteRenderer sr;
    private Collider2D Col;
    private Sprite s;
    public Sprite s1;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        Col = GetComponent<Collider2D>();
        s = sr.sprite;
    }

    void Update()
    {
         if(hiding)
         {
          if(counter < hide)
           {
           counter+= Time.deltaTime;
           }
          else
           counter = hide;
          if(counter == hide)
           Hide();
         }

         if(seeing)
         {
          if(counter < appear)
           {
           counter+= Time.deltaTime;
           }
          else
           counter = appear;
          if(counter == appear)
           Appear();
         }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            hiding = true;
            counter = 0;
        }
    }


    private void Hide()
    {
       Debug.Log("Broken");
       sr.sprite = s1;
       Col.enabled = false;
       hiding = false;
       seeing = true;
       counter = 0;
    }

    private void Appear()
    {
       Debug.Log("Fixed");
       sr.sprite = s;
       Col.enabled = true;
       seeing = false;
    }
}
