using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIRestart : MonoBehaviour
{
    Text restartIndicator;
    [SerializeField] FloatVariable playerHealth;
    private void Start()
    {
        restartIndicator = GetComponent<Text>();
        restartIndicator.enabled = false;
    }

    private void Update()
    {
        if (playerHealth.Value <= 0)
        {
            Time.timeScale = 0.5f;
            restartIndicator.enabled = true;

            if(Input.GetButtonDown("Start"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
