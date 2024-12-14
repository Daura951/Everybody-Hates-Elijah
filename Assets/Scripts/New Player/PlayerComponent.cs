using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    protected InputManager inputManager;
    protected AnimatorController animController;
    protected PlayerState playerState;

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();

    public virtual void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;
    }

}
