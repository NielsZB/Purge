using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Health : Damagable
{
    [SerializeField] float health = 10f;
    [ShowNativeProperty] public float CurrentHealth { get; private set; }
    //[SerializeField] float siphonAmount = 0f;
    //[SerializeField] float siphonDuration = 0f;
    [SerializeField] Gradient hitGradient;
    [SerializeField] Gradient deathGradient;

    // public float SiphonAmount { get { return siphonAmount; } }
    public bool Dead { get; private set; }
    public bool Siphonable { get; private set; }

    public bool IsVulnerable = true;
    // WaitForSeconds waitSeconds;


    public Renderer render;

    MaterialPropertyBlock materialPropertyBlock;
    private void Awake()
    {
        Dead = false;
        CurrentHealth = health;
        //waitSeconds = new WaitForSeconds(siphonDuration);
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    public override void TakeDamage(float damage)
    {
        if (IsVulnerable)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                StartCoroutine(ShowDeath());
                Dead = true;
                //StartCoroutine(SiphonWindow());
            }
            else
            {
                StartCoroutine(ShowHit());
            }
            Debug.Log($"{name} has taken {damage} and has {CurrentHealth} left!");
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

    //IEnumerator SiphonWindow()
    //{
    //    Siphonable = true;
    //    yield return waitSeconds;
    //    Siphonable = false;
    //}

    IEnumerator ShowHit()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / 0.25f;
            render.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_BaseColor", hitGradient.Evaluate(t));
            render.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
    }

    IEnumerator ShowDeath()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / 0.25f;
            render.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_BaseColor", deathGradient.Evaluate(t));
            render.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
        Destroy(gameObject);
    }
}