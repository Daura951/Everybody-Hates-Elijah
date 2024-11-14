using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform otherDoor;
    private InputManager inputManager;

    private Fader fader;

    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.Find("Player UI").transform.Find("Fader").GetComponent<Fader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getInputManager();

            if(inputManager.moveVertical > .5f)
            {
                fader.setPositionToGoTo(otherDoor.position);
                fader.Fade("position");
            }
        }
    }
}
