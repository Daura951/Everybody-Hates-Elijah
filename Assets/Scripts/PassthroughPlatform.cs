using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughPlatform : MonoBehaviour
{
    private Collider2D platformCollider;
    private bool playerOnPlatform;
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        platformCollider = GetComponent<Collider2D>();   
    }

    // Update is called once per frame
    void Update()
    {

        if(playerOnPlatform && inputManager != null && inputManager.moveVertical < 0)
        {
            print("Player passing through platform!!!!");
            platformCollider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool val)
    {
        var player = other.gameObject.GetComponent<Player>();

        if(player != null)
        {
            playerOnPlatform = val;
            inputManager = player.getInputManager();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, false);
    }
}
