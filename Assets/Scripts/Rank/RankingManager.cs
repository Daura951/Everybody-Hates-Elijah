using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RankingManager : MonoBehaviour
{
    private string bestTime;
    private int bestCombo;
    private int bestEnemiesKilled;

    private float playerTime;
    private int playerCombo;
    private int playerEnemiesKilled;

    private TMP_Text timeText;
    private TMP_Text comboText;
    private TMP_Text enemiesKilledText;
    private TMP_Text rankText;
    private TMP_Text continueText;

    private float WaitTime = 5;
    private bool next = false;
    // Start is called before the first frame update
    void Start()
    {
        SetRankingFields();
        next = false;
        timeText = GameObject.Find("timeText").GetComponent<TMP_Text>();
        comboText = GameObject.Find("comboText").GetComponent<TMP_Text>();
        enemiesKilledText = GameObject.Find("enemiesKilledText").GetComponent<TMP_Text>();
        rankText = GameObject.Find("rankText").GetComponent<TMP_Text>();
        continueText = GameObject.Find("continueText").GetComponent<TMP_Text>();
        rankText.text =  calculateRank();
        StartCoroutine(NextLevel());
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && next)
        {
            print("ready");
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(WaitTime);
        continueText.SetText("Press 'Space' to continue");
        next = true;

    }


    float ConvertStringToSeconds(string time)
    {
        string[] parts = time.Split(":");

        return (float.Parse(parts[0]) * 60) + float.Parse(parts[1]);
    }

    string calculateRank()
    {
        print("Combo: " + playerCombo);
        print("enemies killed " + playerEnemiesKilled);

        float bestTimeInSec = ConvertStringToSeconds(bestTime);
        float bestTimePercent = 0;
        bestTimePercent = CalculatePercent(bestTimeInSec, playerTime);

        
        
        float bestComboPercent = CalculatePercent((float)playerCombo,  bestCombo);
        float bestEnemiesKilledPercent = CalculatePercent((float)playerEnemiesKilled, bestEnemiesKilled);

        SetText("Time", playerTime, bestTimeInSec, bestTimePercent, timeText);
        SetText("Max Combo", playerCombo, bestCombo, bestComboPercent, comboText);
        SetText("Enemies Killed", playerEnemiesKilled, bestEnemiesKilled, bestEnemiesKilledPercent, enemiesKilledText);

        float rank = bestTimePercent + bestComboPercent + bestEnemiesKilledPercent;
        rank /= 3;
        print(rank);

        if (rank <= 100 && rank > 75)
        {
            return "Ignovised";
        }
        else if (rank <= 75 && rank > 50)
        {
            return "Metamised";
        }
        else if (rank <= 50 && rank > 25)
        {
            return "Mallodite";
        }
        else return "Drunt";

        
    }

    void SetText(string type, float playerVal, float bestVal,float percent, TMP_Text text)
    {
        text.text = type + ":\t" + playerVal + "\t" + bestVal + ": " + percent+'%';
    }

    void SetText(string type, int playerVal, int bestVal, float percent, TMP_Text text)
    {
        text.text = type + ":\t" + playerVal + "\t" + bestVal + ": " + percent+'%';
    }


    float CalculatePercent(float playerVal, float bestVal)
    {
        return (float)Math.Round(CapPercent(playerVal / bestVal)*100, 2);
    }

    public float CapPercent(float value)
    {
        return (value < 0) ? 0.0f : (value > 1) ? 1.0f : value;
    }

    void SetRankingFields()
    {
        bestTime = PlayerPrefs.GetString("bestTime");
        bestCombo = PlayerPrefs.GetInt("bestCombo");
        bestEnemiesKilled = PlayerPrefs.GetInt("bestEnemiesKilled");

        playerTime = PlayerPrefs.GetFloat("endTime");
        playerCombo = PlayerPrefs.GetInt("comboScore");
        playerEnemiesKilled = PlayerPrefs.GetInt("enemiesKilled");
    }
}

