using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent bladebound = new UnityEvent();
    public static UnityEvent bladeboundEnd = new UnityEvent();

    public static void TriggerBladebound()
    {
        Time.timeScale = 0.5f;
        bladebound?.Invoke();
    }

    public static void TriggerEndOfBladebound()
    {
        Time.timeScale = 1.0f;
        bladeboundEnd?.Invoke();
    }
}
