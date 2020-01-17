using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIImageFill : MonoBehaviour
{
    [SerializeField] FloatVariable value;

    float currentValue;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        currentValue = value.Value;
        image.fillAmount = currentValue;
    }

    private void Update()
    {
        if (currentValue != value.Value)
        {
            currentValue = value.Value;
            image.fillAmount = currentValue;
        }
    }
}
