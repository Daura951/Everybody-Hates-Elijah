using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float health;

    public Slider slider;
    public Image healthBar;
    public AudioSource AS;

    [Header("Hurt Sound")]
    public float lowLim;
    public float medLim;
    public AudioClip lh1;
    public AudioClip lh2;
    public AudioClip lh3;
    public AudioClip grunt;
    public AudioClip mh1;
    public AudioClip mh2;
    public AudioClip mh3;
    public AudioClip hh1;
    public AudioClip hh2;
    public AudioClip hh3;
    public AudioClip Death;



    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        slider.maxValue = 1;
        slider.value = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            health = 0;
            lives--;
            if (lives != 0)
                AS.PlayOneShot(Death);
        }


        if (lives == 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        slider.value = health / MaxHealth;

        if(health > MaxHealth)
            health = MaxHealth;

        if(slider.value > .6f)
        {
            healthBar.color = new Color(0, 255, 0, 1);
        }
        if (slider.value <= .6f && slider.value > .3f)
        {
            healthBar.color = new Color(255, 255, 0, 1);
        }
        else if (slider.value <= .3f)
        {
            healthBar.color = new Color(255, 0, 0, 1);
        }
    }

    public void TakeDamage(float hurt)
    {
      health -= hurt;

        if (!AS.isPlaying)
        {


            if (hurt < lowLim && health >= MaxHealth*.5)
            {
                float pick = Random.Range(1, 7);
                Debug.Log(pick);
                if (pick == 1)
                    AS.PlayOneShot(lh1, 1f);
                if (pick == 2)
                    AS.PlayOneShot(lh2, 1f);
                if (pick == 3)
                    AS.PlayOneShot(lh3, 1f);
                if (pick == 4)
                    AS.PlayOneShot(grunt, 1f);
            }

            else if ((hurt < medLim && health >= MaxHealth * .5) || (hurt < lowLim && health < MaxHealth * .5) )
            {
                float pick = Random.Range(1, 5);
                Debug.Log(pick);

                if (pick == 1)
                    AS.PlayOneShot(mh1, 1f);
                if (pick == 2)
                    AS.PlayOneShot(mh2, 1f);
                if (pick == 3)
                    AS.PlayOneShot(mh3, 1f);
            }

            else if ((hurt < medLim && health <= MaxHealth * .5) || hurt >= medLim)
            {
                float pick = Random.Range(1, 4);
                Debug.Log(pick);

                if (pick == 1)
                    AS.PlayOneShot(hh1, 1f);
                if (pick == 2)
                    AS.PlayOneShot(hh2, 1f);
                if (pick == 3)
                    AS.PlayOneShot(hh3, 1f);
            }

        }


        PlayerAttack.attackInstance.isExecutedOnce = false;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetLives()
    {
        return lives;
    }

    public void ReHeal()
    {
        health = MaxHealth;
    }

    public void Heal(float H)
    {
        health += H;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }
}
