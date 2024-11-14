using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

    private float currentHealth;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private Slider healthBarSlider;

    [SerializeField]
    private Color[] colors;

    [SerializeField]
    private Image heathBarImage;

    [SerializeField]
    private Animator healthAnim;

    [SerializeField]
    private Sprite[] elijahEmotes;

    [SerializeField]
    private Image elijahImage;
    
    [SerializeField]
    private SpriteRenderer render;

    private bool isCoroutineStarted;

    public override void OnStart()
    {
        currentHealth = maxHealth;
        healthBarSlider.value = currentHealth / maxHealth;
        healthAnim.SetFloat("animSpeedParameter", .5f);
    }


    public override void OnUpdate()
    {
        if(healthBarSlider.value <= 0.3f)
        {
            healthAnim.Play("Hurt");
            if (!isCoroutineStarted)
            {
                isCoroutineStarted = true;
                StartCoroutine(FlashRed());
            }
        }
    }
    public override void OnFixedUpdate()
    {

    }

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject collidedGO = collision.gameObject;
        if(collidedGO.GetComponent<Stun_Info>()!= null)
        {
            print("HAZARD!");
            TakeHit(collidedGO.GetComponent<Stun_Info>().GetDAKTInfo()[0]);
            ApplyHealthUI();
            if (currentHealth <= 0)
            {
                EventManager.TriggerOnDeath();
            }
        }
    }

    private void TakeHit(float damage)
    {
        currentHealth -= damage;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHealth < 0)
            currentHealth = 0;

        healthBarSlider.value = currentHealth / maxHealth;
    }

    private void ApplyHealthUI()
    {
        float healthRatio = healthBarSlider.value;

        if (healthRatio > .6f)
        {
            healthAnim.Play("Hurt");
            elijahImage.sprite = elijahEmotes[0];
            heathBarImage.color = colors[0];
        }
        if (healthRatio <= .6f && healthRatio > .3f)
        {
            healthAnim.Play("Hurt");
            elijahImage.sprite = elijahEmotes[1];
            heathBarImage.color = colors[1];
        }
        else if (healthRatio <= .3f)
        {
            healthAnim.SetFloat("animSpeedParameter", .4f);
            elijahImage.sprite = elijahEmotes[2];
            heathBarImage.color = colors[2];
        }
    }

    public IEnumerator FlashRed()
    {
        render.color = new Color(255f, 0f, 0f, 255f);
        yield return new WaitForSeconds(.25f);
        render.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(.25f);
        isCoroutineStarted = false;
    }
}

