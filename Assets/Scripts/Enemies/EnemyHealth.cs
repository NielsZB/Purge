using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;

    public bool IsAlive { get; private set; } = true;
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = health;
    }
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if(CurrentHealth < 0)
        {
            IsAlive = false;
        }
    }
}
