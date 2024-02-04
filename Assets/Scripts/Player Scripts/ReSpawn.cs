using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    Health H;
    PlayerMovement PM;
    Rigidbody2D rb; 
    Stun s;
    Animator anim;
    AudioSource AS;
    Vector2 ReSpawnPoint;
    float L;


    // Start is called before the first frame update
    void Start()
    {
      H = GetComponent<Health>();
      PM = GetComponent<PlayerMovement>();
        AS = GetComponent<AudioSource>();
      rb = GetComponent<Rigidbody2D>();
      s = GetComponent<Stun>();
      anim = GetComponent<Animator>();
      L = H.GetLives();
      ReSpawnPoint = new Vector2(PM.transform.position.x , PM.transform.position.y); 
    }

    // Update is called once per frame
    void Update()
    {
        if(H.GetLives()>0 && L>H.GetLives())
        {
        L=H.GetLives();
        rb.velocity = new Vector2(0f , 0f);
        

        if(s.Stunned)
        {
            anim.SetBool("Stunned",!s.Stunned);

         s.Stunned = false;
        }
            print("Respawn");
                PM.transform.position = ReSpawnPoint;
                H.ReHeal();
            
        }
    }

    public void NewCheckPoint(Vector2 c)
    {
        ReSpawnPoint = c ;
    }
}
