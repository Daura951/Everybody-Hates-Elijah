using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public List<PlayerComponent> playerComponents;
    private AnimatorController animController;
    private PlayerState Playerstate;

    private InputManager inputManager;
    private AttackManager attackManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animController = GetComponent<AnimatorController>();
        Playerstate = GetComponent<PlayerState>();
        attackManager = GetComponent<AttackManager>();
        ConfiugrePlayer(playerComponents);
        

    }

    private void Start()
    {

        foreach(PlayerComponent pc in playerComponents)
        {
            pc.OnStart();
        }

    }


    // Update is called once per frame
    private void Update()
    {
        foreach (PlayerComponent pc in playerComponents)
        {
            pc.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (PlayerComponent pc in playerComponents)
        {
            pc.OnFixedUpdate();
        }
    }

    private void ConfiugrePlayer(List<PlayerComponent> pcs)
    {

        foreach (PlayerComponent pc in pcs)
        {
            pc.player = this;
            pc.Configure(inputManager, animController, Playerstate);
        }
    }

    public PlayerState GetPlayerState()
    {
        return Playerstate;
    }

    public InputManager getInputManager()
    {
        return inputManager;
    }

    public AttackManager GetAttackManager()
    {
        return attackManager;
    }

    public T GetControllers<T>() where T : PlayerComponent
    {
        return playerComponents.OfType<T>().FirstOrDefault();
    }
}
