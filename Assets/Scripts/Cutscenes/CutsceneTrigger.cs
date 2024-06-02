using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    public bool isTriggered = false;

    private GameObject player;
    private GameObject[] enemies;

    private GameObject cutSceneUi;

    private FinishLine finishLine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cutSceneUi = GameObject.Find("Cutscene UI");
        cutSceneUi.SetActive(false);

        finishLine = GameObject.Find("finish").GetComponent<FinishLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTriggered)
        {
            TransitionCutscene();
            isTriggered = !isTriggered;
        }
    }

    private void TransitionCutscene()
    {
        cutSceneUi.SetActive(true);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (Transform child in cutSceneUi.transform)
        {
            child.gameObject.SetActive(false);
        }

        player.GetComponent<PlayerMovement>().isInCutscene = true;
        player.GetComponent<PlayerAttack>().isInCutscene = true;
        finishLine.isInCutscene = true;

        for (int i = 0; i < enemies.Length; i++)
        {
            switch (enemies[i].name)
            {
                case "Male Student":
                    enemies[i].GetComponent<AngryStudent>().isInCutscene = true;
                    break;
                case "TechKid projectile":
                    enemies[i].GetComponent<TechKidProjectile>().isInCutScene = true;
                    break;
            }
        }
    }
}
