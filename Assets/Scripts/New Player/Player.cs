using System;
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

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        animController = GetComponent<AnimatorController>();
        Playerstate = GetComponent<PlayerState>();

        foreach(PlayerComponent pc in playerComponents)
        {
            ConfiugrePlayer(pc);
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

    private void ConfiugrePlayer(PlayerComponent pc)
    {
        pc.Configure(inputManager, animController, Playerstate);
    }
}
