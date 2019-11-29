using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Damagable
{
    [SerializeField] float health = 10f;
    [SerializeField] float siphonAmount = 0f;
    [SerializeField] float siphonDuration = 0f;
    public float CurrentHealth { get; private set; }

    public float SiphonAmount { get { return siphonAmount; } }
    public bool Dead { get; private set; }
    public bool Siphonable { get; private set; }

    WaitForSeconds waitSeconds;

    private void Awake()
    {
        Dead = false;
        CurrentHealth = health;
        waitSeconds = new WaitForSeconds(siphonDuration);
    }

    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            Dead = true;
            StartCoroutine(SiphonWindow());
        }
    }
    public override void TakeDamage(float damage, Vector3 direction)
    {
        TakeDamage(damage);
    }

    public void GainHealth(float health)
    {

        CurrentHealth += health;
        if (CurrentHealth > this.health)
        {
            CurrentHealth = this.health;
        }
    }

    public void AddToMaxHealth(float amount)
    {
        health += amount;
    }

    IEnumerator SiphonWindow()
    {
        Siphonable = true;
        yield return waitSeconds;
        Siphonable = false;
    }
}