using UnityEngine;

public class BladeboundController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

    [SerializeField]
    private int hitsToBladebound;

    private int curHitAmt = 0;

    private bool isInBB = false;
    private bool isBBReady = false;
    private bool lockAddition = false;

    [SerializeField]
    private float totalbladeboundTime = 10.0f;
    private float bladeboundTime = 0.0f;
    public int CurHitAmt { get => curHitAmt; set => curHitAmt = value; }

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;

        inputManager.OnSpecialPressed+=BladeboundInput; 
    }


    public override void OnFixedUpdate()
    {

    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {

        curHitAmt = lockAddition ? 0 : curHitAmt;

        if(!isBBReady && curHitAmt == hitsToBladebound)
        {
            print("WE CAN ACTIVATE IT");
            isBBReady = true;
            
        }

        if(isInBB)
        {
            if(bladeboundTime <= totalbladeboundTime)
            {
                bladeboundTime += Time.deltaTime * 2;
            }
            else
            {
                EventManager.TriggerEndOfBladebound();
                print("END BLADEBOUND!");
                isInBB = false;
                isBBReady = false;
                lockAddition = false;
                curHitAmt = 0;
                bladeboundTime = 0.0f;
            }
        }
        SendDataToPlayerState();
    }

    private void SendDataToPlayerState()
    {
        playerState.isInBB = isInBB;
        playerState.isBBReady = isBBReady;
    }

    private void BladeboundInput()
    {
        if(isBBReady && !isInBB)
        {
            isInBB = true;
            isBBReady = false;
            lockAddition = true;
            animController.Play(Animations.BLADEBOUND_ACTIVATION, false, false);
            EventManager.TriggerBladebound();
            print("BLADEBOUND!");
        }
    }
}
