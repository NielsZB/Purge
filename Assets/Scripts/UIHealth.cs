using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    Slider healthIndicator;
    [SerializeField] FloatVariable playerHealth;
    private void Start()
    {
        healthIndicator = GetComponent<Slider>();
        healthIndicator.maxValue = maxHealth;
    }

    private void Update()
    {
        if (playerHealth.Value > 0)
        {
            healthIndicator.value = playerHealth.Value;
        }
    }
}
