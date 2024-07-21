using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneFader : Fader
{
    public override void OnFade()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }
}
