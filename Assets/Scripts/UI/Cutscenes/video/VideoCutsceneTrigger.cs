using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoCutsceneTrigger : MonoBehaviour
{

    public bool isTransitioningToNewScene;

    public int videoIndex;
    public int sceneToGoTo;
    public VideoCutsceneFader fader;
    private GameObject cutSceneUi;

    public string cutsceneKey;
    private void Awake()
    {
        cutsceneKey = "cutscene_" + videoIndex;
    }


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;

        if(SceneManager.GetActiveScene().buildIndex != 0) { 
            fader = GameObject.Find("Player UI").transform.Find("Fader").GetComponent<VideoCutsceneFader>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player" && PlayerPrefs.GetInt(cutsceneKey) == 0)
        {
            collision.GetComponent<PlayerMovement>().isInCutscene = true;
            collision.GetComponent<PlayerAttack>().isInCutscene = true;
            fader.gameObject.SetActive(true);
            fader.SetVideoIndex(videoIndex);

            if (isTransitioningToNewScene)
            {
                fader.SetSceneToGoTo(sceneToGoTo);
            }
            else
            {
                PlayerPrefs.SetFloat("playerPosX", GameObject.FindGameObjectWithTag("Player").transform.position.x);
                PlayerPrefs.SetFloat("playerPosY", GameObject.FindGameObjectWithTag("Player").transform.position.y);
                fader.SetSceneToGoTo(SceneManager.GetActiveScene().buildIndex);
            }
            PlayerPrefs.SetInt("goToStartMenu", 0);
            print("GoToStartMenu: " + PlayerPrefs.GetInt("goToStartMenu"));



            fader.GetComponent<Animator>().Play("Fade");
            PlayerPrefs.SetInt(cutsceneKey, 1);
        }
    }


    //public void CheckIfShouldBeActive()
    //{
    //    cutsceneKey = "cutscene_" + videoIndex;
    //    print(cutsceneKey+ " "+ PlayerPrefs.GetInt(cutsceneKey));
    //    if (PlayerPrefs.GetInt(cutsceneKey) == 1)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //    gameObject.SetActive(true);
    //}

}
