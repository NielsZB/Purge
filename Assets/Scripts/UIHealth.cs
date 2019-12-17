using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealth : MonoBehaviour
{
    Text healthIndicator;
    [SerializeField] FloatVariable playerHealth;
    private void Start()
    {
        healthIndicator = GetComponent<Text>();
    }

    private void Update()
    {
        if (playerHealth.Value > 0)
        {
            healthIndicator.text = playerHealth.Value.ToString();
        }
        else
        {
            healthIndicator.color = Color.red;
            healthIndicator.text = "Dead";
        }
    }
}
