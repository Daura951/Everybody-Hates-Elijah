using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeSliderController : MonoBehaviour
{
    public Slider slider;

    public delegate void OnSliderLoad(float initial_value);
    public static event OnSliderLoad onSliderLoad;

    private void Start()
    {
        slider = GetComponent<Slider>();
        onSliderLoad.Invoke(slider.value);
    }

    public void onClickPlusOne()
    {
        slider.value += 1;
    }
    public void onClickMinusOne()
    {
        slider.value -= 1;
    }
}
