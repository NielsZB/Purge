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

    Animator animator;
    PlayerController controller;
    Movement movement;
    private void Start()
    {
        CurrentHealth = health;
        Health.Set(health.Remap01(0f,health));
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        movement = GetComponent<Movement>();
    }
    public void TakeDamage(float amount)
    {
        if (IsInvulnerable)
            return;

        CurrentHealth -= amount;
        if(Health!=null){
            Health.Set(CurrentHealth.Remap01(0f, health));
        }
        if(CurrentHealth <= 0)
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            controller.DisableControls();
        }
        else
        {
            movement.ChangeSpeeds();
            animator.SetTrigger("Hit");

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
