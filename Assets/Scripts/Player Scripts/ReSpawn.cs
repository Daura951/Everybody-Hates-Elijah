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
   public CameraSwitch ActiveArena;
    Vector2 ReSpawnPoint;

    public bool Waitroom = false;
    public bool complete = false;

    [Header("Death Cries")]
    public AudioClip Death1;
    public AudioClip Death2;
    public AudioClip Death3;
    public AudioClip Death4;
    public AudioClip Death5;


    // Start is called before the first frame update
    void Start()
    {
      H = GetComponent<Health>();
      PM = GetComponent<PlayerMovement>();
        AS = GetComponent<AudioSource>();
      rb = GetComponent<Rigidbody2D>();
      s = GetComponent<Stun>();
      anim = GetComponent<Animator>();
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
                float pick = Random.Range(1, 6);
                if(pick == 1)
                {
                AS.PlayOneShot(Death1);
                StartCoroutine(ScreamOfDeath(Death1.length));
                }
                if (pick == 2)
                {
                    AS.PlayOneShot(Death2);
                    StartCoroutine(ScreamOfDeath(Death2.length));
                }
                if (pick == 3)
                {
                    AS.PlayOneShot(Death3);
                    StartCoroutine(ScreamOfDeath(Death3.length));
                }
                if (pick == 4)
                {
                    AS.PlayOneShot(Death4);
                    StartCoroutine(ScreamOfDeath(Death4.length));
                }
                if (pick == 5)
                {
                    AS.PlayOneShot(Death5);
                    StartCoroutine(ScreamOfDeath(Death5.length));
                }


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
