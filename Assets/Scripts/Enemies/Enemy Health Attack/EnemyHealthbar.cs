using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    Camera camera;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    GameObject fill;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position+offset;
    }

    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;

        if(slider.value <= 0)
        {
            fill.SetActive(false);
        }
    }
}
