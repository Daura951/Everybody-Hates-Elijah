using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoCutsceneTrigger : MonoBehaviour
{

    public int videoIndex;
    public int sceneToGoTo;
    private VideoCutsceneFader fader;
    private GameObject cutSceneUi;
    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.Find("Player UI").transform.Find("Fader").GetComponent<VideoCutsceneFader>();
        fader.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.GetComponent<PlayerMovement>().isInCutscene = true;
            collision.GetComponent<PlayerAttack>().isInCutscene = true;
            fader.gameObject.SetActive(true);
            fader.SetVideoIndex(videoIndex);
            fader.SetSceneToGoTo(sceneToGoTo);
            fader.GetComponent<Animator>().Play("Fade");
                
        }
    }
}
