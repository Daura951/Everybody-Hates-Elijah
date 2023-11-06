using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float MaxHealth;
    public float health;
    public Slider healthBar;



    // Start is called before the first frame update
    void Start()
    {
      health = MaxHealth;  
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health / MaxHealth;
        if(health <= 0)
        {
            health = 0;
        }
    }

    void FixedUpdate()
    {
        if(health > MaxHealth)
            health = MaxHealth;
    }

    public void TakeDamage(float hurt)
    {
      health -= hurt;
    }

    public float GetHealth()
    {
        return health;
    }

    public void ReHeal()
    {
        health = MaxHealth;
    }
}
