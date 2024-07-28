using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public int NextScene;
    private float timeSec = 0f;
    private float timeMin = 0f;
    private float rankedTime = 0f;
    private bool stopTimer;
    public TMP_Text timerText;

    public bool isInCutscene = false;

    public RankData rankValues;

    public GeneralSceneFader fader;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvasObject = GameObject.Find("Player UI");
        timerText = canvasObject.transform.Find("Timer").GetComponent<TMP_Text>();
        PlayerPrefs.SetString("bestTime", rankValues.BestTime);
        PlayerPrefs.SetInt("bestCombo", rankValues.BestCombo);
        PlayerPrefs.SetInt("bestEnemiesKilled", rankValues.BestEnemiesKilled);

    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer && !isInCutscene)
        {
            timeSec += Time.deltaTime;
            rankedTime += Time.deltaTime;

            if(timeSec >= 60)
            {
                timeMin++;
                timeSec = 0f;
            }

            string timeSecStr = (timeSec < 10) ? "0" + (int)timeSec : ((int)timeSec).ToString();
            string timeMinStr = (timeMin < 10) ? "0" + (int)timeMin : ((int)timeMin).ToString();

            timerText.text = "" + timeMinStr + ":" +timeSecStr;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stopTimer = true;
            PlayerPrefs.SetFloat("endTime", rankedTime);
            fader.gameObject.SetActive(true);
            LoaderCallback.targetScene = NextScene;
            fader.SetSceneToGoTo(3);
            fader.GetComponent<Animator>().Play("Fade");
        }
    }
}
