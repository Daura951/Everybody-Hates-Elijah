using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float food;

    public float GetFood()
    {
        return food;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        { 
            Health H = col.GetComponent<Health>();
            H.Heal(food);
            Destroy(this.gameObject);
        }
    }
}
