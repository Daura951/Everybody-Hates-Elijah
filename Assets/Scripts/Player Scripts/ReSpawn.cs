using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Stun stun;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public CameraSwitch ActiveArena;

    private Vector3 ReSpawnPoint;

    public bool Waitroom = false;
    public bool complete = false;

    [Header("Death Cries")]
    public AudioClip[] Death;


    // Start is called before the first frame update
    void Start()
    {
        ReSpawnPoint = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Refactor to check for death on events such as "OnTakeDamage" instead polling on update all the time 
        // polling on update for everything really bogs down the game in the long run
        if (health.dead)
        {
            if (!Waitroom)
            {
                Waitroom = true;
                health.lives--;
                int pick = Random.Range(0, Death.Length);
                animator.SetFloat("SinkSpeed", (1 / (Death[pick].length * 4)));
                audioSource.PlayOneShot(Death[pick]);
                StartCoroutine(ScreamOfDeath(Death[pick].length));
            }

            if (health.lives > 0 && complete)
            {
                gameObject.transform.position = ReSpawnPoint;
                spriteRenderer.enabled = true;
                rigidBody.velocity = Vector2.zero;
                if (stun.Stunned)
                {
                    animator.SetBool("Stunned", !stun.Stunned);

                    stun.Stunned = false;
                }
                if (ActiveArena != null)
                {
                    ActiveArena.Reset();
                }

                animator.SetBool("Sinking", false);
                rigidBody.constraints = RigidbodyConstraints2D.None;
                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

                Debug.Log(complete);
                Debug.Log(health.lives);
                Debug.Log("I think I am dead");
                health.ReHeal();
                stun.EnablePit();
                Waitroom = complete = false;
                

            }
            if (complete && health.lives == 0)
            {
                print("DIE DIE DIE");
                // TODO: Throw a game Over event instead and have something catch it to bring us to a game over scene
                // or disable the player's model or something else. Destroying the game object breaks things
                Destroy(this.gameObject);
            }


        }
    }

    public void NewCheckPoint(Vector3 position)
    {
        ReSpawnPoint = position;
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
