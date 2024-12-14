using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboController : PlayerComponent
{

    public float ComboTimer = 3f;
    private bool combodetect = true;
    private float comboScore;
    private float timer, comboTimerStored;
    public TMP_Text comboText;

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);

    }

    public override void OnStart()
    {
        comboTimerStored = ComboTimer;
        comboText.gameObject.SetActive(false);
    }

    public override void OnFixedUpdate()
    {

    }


    public override void OnUpdate()
    {
    if (!playerState.isInCutscene) { DepleteComboTimer(); }
        SendDataToPlayerState();
    }

    private void SendDataToPlayerState()
    {

    }

    void DepleteComboTimer()
    {
    if (comboScore > 0)
    {
        if (comboScore >= 3)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = (comboScore < 10 ? "0" : "") + comboScore;
        }

        if (timer >= ComboTimer)
        {
            timer = ComboTimer;
        }

        if (timer == ComboTimer)
        {
            print("Combo score: " + comboScore);
            comboScore = 0;
            ComboTimer = comboTimerStored;
        }

        else timer += Time.deltaTime;
    }

    else
    {
        comboText.text = "00";
        comboText.gameObject.SetActive(false);
    }

    }

    // This function need to be called when an enemy is damaged
    public void Combo()
    {
    //print("Combo Time");
    timer = 0f;
    if (combodetect)
    {
        combodetect = false;
        comboScore++;
        StartCoroutine(ComboDelay());
    }

    if (comboScore < 21)
        ComboTimer += .1f;
    }
    IEnumerator ComboDelay()
    {
    yield return new WaitForSeconds(0.015f);
    combodetect = true;
    }


}
