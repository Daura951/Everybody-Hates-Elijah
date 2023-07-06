using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    public float attack , retract , load;
    private float attackT , retractT , loadT;

    private string State;
    private bool spawn = false , active;

    [SerializeField] private GameObject item;
    private GameObject clone;

    SpearActivator SA;
    [SerializeField] GameObject lever;

    public Vector3 move;
    public float ang;

    void Start()
    {
        attackT = attack;
        retractT = retract;
        loadT = load;

        SA = lever.GetComponent<SpearActivator>();
    }

    void Update()
    {
        active = SA.GetIsActive();

        
        switch (State)
        {
            case "Extend":
             if(attackT > 0)
              attackT -= Time.deltaTime;
             else
              attackT = 0;
             if(attackT == 0)
              summon();
             break;

            case "Retract":
             if(retractT > 0)
              retractT -= Time.deltaTime; 
             else
              retractT = 0;
             if(retractT == 0)
              Delete();
             break;

            case "Reload":
             if(loadT > 0)
              loadT-= Time.deltaTime; 
             else
              loadT = 0;
             if(loadT == 0)
              Reload();
             break; 

        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && spawn == false && clone == null && active)
        {
            spawn = true;
            State = "Extend";
        }
    }

  /*  private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            
        }
    }  */

    private void summon()
    {
       clone = Instantiate(item , transform.position + move , Quaternion.Euler(0,180,ang));
       State = "Retract";
       Debug.Log("summon");
    }

    private void Delete()
    {
       Destroy(clone);
       Debug.Log("delted");
        State = "Reload";
    }

    private void Reload()
    {
        State = "";
        spawn = false;
        attackT = attack;
        retractT = retract;
        loadT = load;
        Debug.Log("restart");
    }
}
