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
    SpriteRenderer SR;
   public CameraSwitch ActiveArena;
    Vector2 ReSpawnPoint;

    public bool Waitroom = false;
    public bool complete = false;

    [Header("Death Cries")]
    public AudioClip[] Death;



    // Start is called before the first frame update
    void Start()
    {
      H = GetComponent<Health>();
      PM = GetComponent<PlayerMovement>();
        AS = GetComponent<AudioSource>();
      rb = GetComponent<Rigidbody2D>();
      s = GetComponent<Stun>();
      anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
      ReSpawnPoint = new Vector2(PM.transform.position.x , PM.transform.position.y); 
    }

    // Update is called once per frame
    void Update()
    {
        if(H.dead)
        {
            if(!Waitroom)
            {
                Waitroom = true;
                H.lives--;
                int pick = Random.Range(0, Death.Length);
                AS.PlayOneShot(Death[pick]);
                StartCoroutine(ScreamOfDeath(Death[pick].length));
            }

            if(complete && H.lives > 0)
            {
                rb.velocity = new Vector2(0f, 0f);
                if (s.Stunned)
                {
                    anim.SetBool("Stunned", !s.Stunned);

                    s.Stunned = false;
                }
                if(ActiveArena != null)
                {
                    ActiveArena.Reset();
                }


                PM.transform.position = ReSpawnPoint;
                H.ReHeal();
                Waitroom = complete = false;
                SR.enabled = true;
            }
            if(complete && H.lives ==0)
            {
                print("DIE DIE DIE");
                Destroy(this.gameObject);
            }


        }
    }

    public void NewCheckPoint(Vector2 c)
    {
        ReSpawnPoint = c ;
    }


    IEnumerator ScreamOfDeath(float d)
    {
        yield return new WaitForSeconds(d);
        complete = true;
    }

    public void AssignArena(CameraSwitch AR)
    {
        ActiveArena = AR;
    }
}
