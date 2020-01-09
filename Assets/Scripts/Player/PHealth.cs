using System.Collections;
using UnityEngine;

public class PHealth : Health
{
    [SerializeField] FloatVariable Health;

    PController controller;
    protected override void Awake()
    {
        base.Awake();
        Health.Set(CurrentHealth);
        controller = GetComponent<PController>();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Health.Set(CurrentHealth);
        if(IsDead)
        {
            controller.DisableControls();
        }
    }

    public void GainHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, health);
        Health.Set(CurrentHealth);
    }

    public void GainMaxHealth(float amount)
    {
        health += amount;
    }
}