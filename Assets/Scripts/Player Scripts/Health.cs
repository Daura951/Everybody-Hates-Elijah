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
    public AudioSource Off;

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
        if (!AS.isPlaying && !Off.isPlaying)
            AS.Play();


        if(health <= 0)
        {
            health = 0;
            lives--;
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
       PlayerAttack.attackInstance.isExecutedOnce = false;
        AS.Pause();
        Off.Play();
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
}
