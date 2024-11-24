using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public int sceneToGoTo;
    public Fader fader;

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        fader = GameObject.Find("Player UI").transform.Find("Fader").GetComponent<Fader>();
        if (fader != null)
        {
            print("Fader found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            fader.SetSceneToGoTo(sceneToGoTo);
            fader.Fade("scene");
        }
    }
}
