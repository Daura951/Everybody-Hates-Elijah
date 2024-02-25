using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingSFX : MonoBehaviour
{
    public AudioSource ASfx;
    public AudioClip[] Hit;
    public AudioClip Miss;
    public float Delay;
          bool hit = false;

    private void OnEnable()
    {
        StartCoroutine(ScreamOfDeath(Delay));
    }

    IEnumerator ScreamOfDeath(float d)
    {
        hit = false;
        yield return new WaitForSeconds(d);

        if(!hit && !ASfx.isPlaying && Miss != null)
            ASfx.PlayOneShot(Miss);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            hit = true;
            if (!ASfx.isPlaying)
            {
                int r = Random.Range(0, Hit.Length);
                ASfx.PlayOneShot(Hit[r]);
            }
        }    
    }
}
