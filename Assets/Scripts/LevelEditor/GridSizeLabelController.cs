using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSizeLabelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;

    private void OnEnable()
    {
        GridSizeSliderController.onSliderLoad += (float initial_value) => onGridSizeSliderUpdate(initial_value);
    }
    public void onGridSizeSliderUpdate(float newValue)
    {
        m_TextMeshProUGUI.text = "Grid Size: " + newValue.ToString();
    }
    private void OnDisable()
    {
        GridSizeSliderController.onSliderLoad -= (float initial_value) => onGridSizeSliderUpdate(initial_value);
    }
}
