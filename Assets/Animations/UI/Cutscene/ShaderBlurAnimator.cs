using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderBlurAnimator : MonoBehaviour
{
    public Material material; // Assign this in the Inspector
    public string propertyName = "_Size"; // Name of the shader property to animate
    public float animationDuration = 0.5f; // Duration of the animation

    public float blurAmt = 5f;

    private float animationTime = 0.0f;
    private bool isAnimating = false;
    public bool isAnimatingEnd = false;

    void Update()
    {
        if (isAnimating)
        {
            animationTime += Time.deltaTime;
            float normalizedTime = animationTime / animationDuration;
            float sizeValue = Mathf.Lerp(0, blurAmt, normalizedTime);
            material.SetFloat(propertyName, sizeValue);

            if (normalizedTime >= blurAmt)
            {
                isAnimating = false;


                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        else if(isAnimatingEnd)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            animationTime += Time.deltaTime;
            float normalizedTime = animationTime / animationDuration;
            float sizeValue = Mathf.Lerp(blurAmt, 0, normalizedTime);
            material.SetFloat(propertyName, sizeValue);

            if (normalizedTime <= 0)
            {
                isAnimatingEnd = false;
            }
        }
    }

    public void StartAnimation()
    {
        animationTime = 0.0f;
        isAnimating = true;
    }

    public void EndCutscene()
    {
        animationTime = 0.0f;
        isAnimatingEnd = true;
    }
}
