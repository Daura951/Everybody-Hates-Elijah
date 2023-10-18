using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] private float MaxHealth;
    [SerializeField] private float health;
    public Sprite s;
    SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            sp.sprite = s;
            GetComponent<Animator>().SetBool("isDead", true);
        }
    }

    void FixedUpdate()
    {
        if (health > MaxHealth)
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


    public void ReHeal()
    {
        health = MaxHealth;
    }

    public void Heal(float H)
    {
        health += H;
    }
}
