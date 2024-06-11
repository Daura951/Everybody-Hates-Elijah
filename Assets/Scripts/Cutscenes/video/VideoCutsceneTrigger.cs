using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoCutsceneTrigger : MonoBehaviour
{

    public int videoIndex;
    public int sceneToGoTo;
    private Fader fader;
    private GameObject cutSceneUi;
    // Start is called before the first frame update
    void Start()
    {
        cutSceneUi = GameObject.Find("Cutscene UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            cutSceneUi.SetActive(true);

            foreach (Transform child in cutSceneUi.transform)
            {
                if (child.name == "Fader")
                {
                    child.gameObject.SetActive(true);
                    fader = child.GetComponent<Fader>();
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }

            collision.GetComponent<PlayerMovement>().isInCutscene = true;
            collision.GetComponent<PlayerAttack>().isInCutscene = true;

            fader.SetVideoIndex(videoIndex);
            fader.SetSceneToGoTo(sceneToGoTo);
            fader.GetComponent<Animator>().Play("VideoCutsceneTransition");
                
        }
    }
}
