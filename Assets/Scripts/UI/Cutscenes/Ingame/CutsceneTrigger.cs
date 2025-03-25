using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public TextAsset txtFile;
    public bool isTriggered = false;
    public bool cutsceneEnded = false;
    public Character[] characters;


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTriggered)
        {
            CutsceneManager.instance.TransitionToCutscene(txtFile, characters, this);
            isTriggered = !isTriggered;
        }

        if(cutsceneEnded)
        {
            CutsceneManager.instance.TransitionBackToGame();
            Destroy(this.gameObject);
        }
    }
}
