using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public List<PlayerComponent> playerComponents;
    [SerializeField]
    private AnimatorController animController;

    [SerializeField]
    private PlayerAttackController playerAttack;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
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

    public InputManager getInputManager()
    {
        return inputManager;
    }

    public AnimatorController getAnimController()
    {
        return animController;
    }

    public PlayerAttackController GetAttackController()
    {
        return playerAttack;
    }
}
