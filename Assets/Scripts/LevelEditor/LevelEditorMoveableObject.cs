using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class LevelEditorMoveableObject : MonoBehaviour
{
    public Renderer myRenderer;

    void CheckForClick()
    {
        Vector3 positionToCheck = LevelEditorMouse.mousePosition;
        positionToCheck.z = myRenderer.bounds.center.z;

        if (myRenderer.bounds.Contains(positionToCheck))
        {
            LevelEditorMouse.selectedObject = gameObject;
        }
    }

    void OnEnable()
    {
        LevelEditorMouse.OnMouseClick += CheckForClick;
    }

    void OnDisable()
    {
        LevelEditorMouse.OnMouseClick -= CheckForClick;
    }
}