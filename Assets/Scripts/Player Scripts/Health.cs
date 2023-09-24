using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float health;


    // Start is called before the first frame update
    void Start()
    {
      health = MaxHealth;  
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            health = 0;
            lives--;
        }

        
            if(lives == 0)
                Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if(health > MaxHealth)
            health = MaxHealth;
    }

    public void TakeDamage(float hurt)
    {
      health -= hurt;
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
}
