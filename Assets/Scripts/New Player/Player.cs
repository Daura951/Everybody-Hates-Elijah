using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public List<PlayerComponent> playerComponents;
    private AnimatorController animController;
    private PlayerAttackController attackController;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        attackController = GetComponent<PlayerAttackController>();
        animController = GetComponent<AnimatorController>();

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
       if(pc is PlayerAttackController)
       {
            ((PlayerAttackController)pc).Configure(inputManager, animController);
       }

       else if(pc is PlayerMovementController)
       {
            ((PlayerMovementController)pc).Configure(inputManager, animController, attackController);
       }
    }
}
