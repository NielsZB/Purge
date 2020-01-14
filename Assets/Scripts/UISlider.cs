using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISlider : MonoBehaviour
{
    [SerializeField] FloatVariable value;
    
    float currentValue;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        currentValue = value.Value;
        slider.value = currentValue;
    }

    private void Update()
    {
        if (currentValue != value.Value)
        {
            currentValue = value.Value;
            slider.value = currentValue;
        }
    }
}
