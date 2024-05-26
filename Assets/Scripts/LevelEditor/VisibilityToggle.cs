using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToToggle;

    public void onToggle(bool value)
    {
        gameObjectToToggle.SetActive(value);
    }
}
