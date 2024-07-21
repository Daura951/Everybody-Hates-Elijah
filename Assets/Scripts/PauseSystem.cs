using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseSystem
{
    public static bool paused = false;

    public static void ChangeGameState()
    {
        switch(paused)
        {
            case false:
             paused = true;
             Time.timeScale = 0;
             break;

            case true:
                paused = false;
                Time.timeScale = 1f;
            break;
        }
    }
}
