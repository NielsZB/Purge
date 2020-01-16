using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable Health;
    [SerializeField] float health;

    public float CurrentHealth { get; protected set; }
    public bool IsAlive { get; private set; }
    public bool IsInvulnerable { get; private set; } = false;

    private void Start()
    {
        CurrentHealth = health;
        Health.Set(health.Remap01(0f,health));
    }
    public void TakeDamage(float amount)
    {
        if (IsInvulnerable)
            return;

        CurrentHealth -= amount;
        Health.Set(CurrentHealth.Remap01(0f, health));
        if(CurrentHealth <= 0)
        {
            IsAlive = false;
        }
    }

    public void EnableInvulnerability()
    {
        IsInvulnerable = true;
    }

    public void DisableInvulnerability()
    {
        IsInvulnerable = false;
    }

    public void ResetHealth(){
        CurrentHealth = health;
    }
}
