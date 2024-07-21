using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Fader : MonoBehaviour
{

    protected int sceneToGoTo;


    public abstract void OnFade();

    public void SetSceneToGoTo(int sceneToGoTo)
    {
        this.sceneToGoTo = sceneToGoTo;
    }

}
