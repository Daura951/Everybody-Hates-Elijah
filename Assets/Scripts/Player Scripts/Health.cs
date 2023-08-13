using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float MaxHealth;
    public float health;


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
            Destroy(this.gameObject);
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
}
