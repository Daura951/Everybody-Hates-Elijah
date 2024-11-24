using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent bladebound = new UnityEvent();
    public static UnityEvent bladeboundEnd = new UnityEvent();
    public static UnityEvent onCutesceneEnter = new UnityEvent();
    public static UnityEvent onCutsceneExit = new UnityEvent();
    public static UnityEvent onDeath = new UnityEvent();
    public static UnityEvent onStunStart = new UnityEvent();
    public static UnityEvent onStunEnd = new UnityEvent();

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

    public static void TriggerOnCutsceneEnter()
    {
        onCutesceneEnter?.Invoke();
    }

    public static void TriggerOnCutsceneExit()
    {
        onCutsceneExit?.Invoke();
    }

    public static void TriggerOnDeath()
    {
        onDeath?.Invoke();
    }

    public static void TriggerOnStunStart()
    {
        onStunStart?.Invoke();
    }

    public static void TriggerOnStunEnd()
    {
        onStunEnd?.Invoke();
    }
}
