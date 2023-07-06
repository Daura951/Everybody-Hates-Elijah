using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearActivator : MonoBehaviour
{

    private bool IsActive = false; 


        private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            IsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            IsActive = false;
        }
    }

    public bool GetIsActive()
  {
  return IsActive;
  }

}
