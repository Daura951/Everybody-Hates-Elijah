using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
     if (col.gameObject.tag == "Player")
      {
      Debug.Log("hit");
      }
    }
}
