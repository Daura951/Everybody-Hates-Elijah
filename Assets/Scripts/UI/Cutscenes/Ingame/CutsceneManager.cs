using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    public static CutsceneManager instance;

    private GameObject[] enemies;

    private GameObject cutSceneUi;

    private FinishLine finishLine;

    private Player player;

    public DialogueParser parser;

    // Start is called before the first frame update
    void Start()
    {

        if(instance == null)
        {
            instance = this;
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        cutSceneUi = GameObject.Find("Cutscene UI");
        cutSceneUi.GetComponent<ShaderBlurAnimator>().animationDuration = 0.1f;
        cutSceneUi.SetActive(false);

        //if(GameObject.Find("FinishTrigger(Clone)") == null)
        //{
        //    finishLine = GameObject.Find("FinishTrigger").GetComponent<FinishLine>();
        //}
        //else finishLine = GameObject.Find("FinishTrigger(Clone)").GetComponent<FinishLine>();

        parser.InitalizeDialogueParser();
    }


    public void TransitionToCutscene(TextAsset txtFile, Character[] characters, CutsceneTrigger currentTrigger)
    {
        parser.txtFile = txtFile;
        parser.characters = characters;
        parser.cutsceneTrigger = currentTrigger;

        cutSceneUi.SetActive(true);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (Transform child in cutSceneUi.transform)
        {
            child.gameObject.SetActive(false);
            if (child.gameObject.name == "CutsceneBG")
            {
                child.gameObject.SetActive(true);
            }
        }

        player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        EventManager.TriggerOnCutsceneEnter();

        if (finishLine != null)
        {
            finishLine.isInCutscene = true;
        }

        for (int i = 0; i < enemies.Length; i++)
        {

            //TODO: Stop the enemies!
            switch (enemies[i].name)
            {

            }
        }

        parser.isInCutscene = true;
        parser.isInit = true;
    }

    public void TransitionBackToGame()
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

    public GameObject GetCutsceneUi()
    {
        return cutSceneUi;
    }

}
