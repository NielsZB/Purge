using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable Health;
    [SerializeField] float health;
    [SerializeField] Gradient hitGradient;
    [SerializeField] Renderer render;
    public float CurrentHealth { get; protected set; }
    public bool IsAlive { get; private set; } = true;
    public bool IsInvulnerable { get; private set; } = false;

    Animator animator;
    PlayerController controller;
    Movement movement;

    MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        CurrentHealth = health;
        Health.Set(health.Remap01(0f,health));
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        movement = GetComponent<Movement>();
        propertyBlock = new MaterialPropertyBlock();
    }
    public void TakeDamage(float amount)
    {
        if (IsInvulnerable)
            return;
        CurrentHealth -= amount;
        StartCoroutine(Hit());
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

    IEnumerator Hit()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 0.25f;
                render.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_EmissiveColor", hitGradient.Evaluate(t));
                render.SetPropertyBlock(propertyBlock);

            yield return null;
        }
    }
}
