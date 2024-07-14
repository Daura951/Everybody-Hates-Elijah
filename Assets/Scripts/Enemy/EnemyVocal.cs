using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVocal : MonoBehaviour
{
    public AudioSource EnemyVoice;
    [SerializeField] AudioSource EnemySFX;
    public float delay;
    [Header("Main")]
    public AudioClip[] MainAwakeSound;
    public AudioClip[] MainDespawnSound;
    public AudioClip[] MainHurtSound;
    public AudioClip[] MainHurtGrunt;
    public AudioClip[] MainAttackedSound;
    public AudioClip[] MainAttackedGrunt;
    [Header("Alts")]
    public AudioClip[] AltAwakeSound;
    public AudioClip[] AltDespawnSound;
    public AudioClip[] AltHurtSound;
    public AudioClip[] AltHurtGrunt;
    public AudioClip[] AltAttackedSound;
    public AudioClip[] AltAttackedGrunt;
    public void RoleCall()
    {
        int f = Random.Range(0, MainAwakeSound.Length);
            EnemyVoice.PlayOneShot(MainAwakeSound[f], 1f);
    }

    public void Attack()
    {
        if(!EnemySFX.isPlaying)
        StartCoroutine(Attacked());
    }

    public void Hurt()
    {
        if (!EnemySFX.isPlaying)
            StartCoroutine(Hurted());
    }

    IEnumerator Attacked()
    {
        int T = Random.Range(0, 3);

        if (T % 2 == 0)
        {
            int f = Random.Range(0, MainAttackedGrunt.Length);
            EnemySFX.PlayOneShot(MainAttackedGrunt[f], 1f);
            if (T == 2)
                yield return new WaitForSeconds(MainAttackedGrunt[f].length + delay);
        }

        if (!EnemyVoice.isPlaying && T > 0)
        {
            int f = Random.Range(0, MainAttackedSound.Length);
            EnemyVoice.PlayOneShot(MainAttackedSound[f], 1f);
        }
    }

    IEnumerator Hurted()
    {
        int T = Random.Range(0, 3);

        if (T%2==0)
        {
            int f = Random.Range(0, MainHurtGrunt.Length);
            EnemySFX.PlayOneShot(MainHurtGrunt[f], 1f);
            if (T == 2)
                yield return new WaitForSeconds(MainHurtGrunt[f].length + delay);
        }

        if (!EnemyVoice.isPlaying && T>0)
        {
            int f = Random.Range(0, MainHurtSound.Length);
            EnemyVoice.PlayOneShot(MainHurtSound[f], 1f);
        }
        
    }

    public float Died()
    {
        if (EnemyVoice.isPlaying)
            EnemyVoice.Stop();
        int f = Random.Range(0, MainDespawnSound.Length);
            EnemyVoice.PlayOneShot(MainDespawnSound[f], 1f);
            return MainDespawnSound[f].length;   
       
    }

    public void Replace()
    {
        MainAwakeSound = AltAwakeSound;
        MainDespawnSound = AltDespawnSound;
        MainAttackedSound = AltAttackedSound;
        MainAttackedGrunt = AltAttackedGrunt;
        MainHurtSound = AltHurtSound;
        MainHurtGrunt = AltHurtGrunt;
    }
}
