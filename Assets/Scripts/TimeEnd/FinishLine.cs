using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishLine : MonoBehaviour
{

    private float timeSec = 0f;
    private float timeMin = 0f;
    private float rankedTime = 0f;
    private bool stopTimer;
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
        {
            timeSec += Time.deltaTime;
            rankedTime += Time.deltaTime;

            if(timeSec >= 60)
            {
                timeMin++;
                timeSec = 0f;
            }
        }

        string timeSecStr = (timeSec < 10) ? "0" + (int)timeSec : ((int)timeSec).ToString();
        string timeMinStr = (timeMin < 10) ? "0" + (int)timeMin : ((int)timeMin).ToString();

        timerText.text = "" + timeMinStr + ":" +timeSecStr;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stopTimer = true;
        print(rankedTime);
        PlayerPrefs.SetFloat("endTime", rankedTime);
        SceneManager.LoadScene(3);
    }
}
