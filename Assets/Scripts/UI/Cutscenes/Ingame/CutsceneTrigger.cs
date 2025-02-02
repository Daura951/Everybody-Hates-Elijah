using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    public bool isTriggered = false;
    public bool cutsceneEnded = false;

    public DialogueParser parser;

    //private GameObject player;
    private GameObject[] enemies;

    private GameObject cutSceneUi;

    private FinishLine finishLine;

    private Player player;


    public float timeBeforeCutscene;

    // Start is called before the first frame update
    void Start()
    {

        this.GetComponent<SpriteRenderer>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        cutSceneUi = GameObject.Find("Cutscene UI");
        cutSceneUi.GetComponent<ShaderBlurAnimator>().animationDuration = timeBeforeCutscene;
        cutSceneUi.SetActive(false);

        //if(GameObject.Find("FinishTrigger(Clone)") == null)
        //{
        //    finishLine = GameObject.Find("FinishTrigger").GetComponent<FinishLine>();
        //}
        //else finishLine = GameObject.Find("FinishTrigger(Clone)").GetComponent<FinishLine>();

        parser.InitalizeDialogueParser();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTriggered)
        {
            TransitionToCutscene();
            isTriggered = !isTriggered;
        }

        if(cutsceneEnded)
        {
            TransitionBackToGame();
            Destroy(this.gameObject);
        }
    }

    private void TransitionToCutscene()
    {
        cutSceneUi.SetActive(true);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (Transform child in cutSceneUi.transform)
        {
            child.gameObject.SetActive(false);
        }

        player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        EventManager.TriggerOnCutsceneEnter();

        if (finishLine != null)
        {
            finishLine.isInCutscene = true;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            switch (enemies[i].name)
            {

            }
        }

        parser.isInCutscene = true;
        parser.isInit = true;
    }

    private void TransitionBackToGame()
    {
        cutSceneUi.SetActive(false);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (Transform child in cutSceneUi.transform)
        {
            child.gameObject.SetActive(false);
        }
        player.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        EventManager.TriggerOnCutsceneExit();


        if (finishLine != null)
        {
            finishLine.isInCutscene = false;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            switch (enemies[i].name)
            {

            }
        }

        parser.isInCutscene = false;
        parser.isInit = false;
    }


    public GameObject getCutsceneUi()
    {
        return cutSceneUi;
    }
}
