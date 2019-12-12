using System.Collections;
using UnityEngine;

public class PHealth : Health
{
    [SerializeField] FloatVariable Health;
    

    protected override void Awake()
    {
        base.Awake();
        Health.Set(CurrentHealth);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Health.Set(CurrentHealth);
        StartCoroutine(ShowImpact(CurrentHealth <= 0));

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

    IEnumerator ShowImpact(bool died)
    {
        float t = 0;

        float duration;
        Gradient gradient = new Gradient();

        if (died)
        {
            IsDead = true;
            duration = 0.5f;
            gradient = deathGradient;
        }
        else
        {
            duration = 0.25f;
            gradient = hitGradient;
        }

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            render.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_BaseColor", gradient.Evaluate(t));
            render.SetPropertyBlock(materialPropertyBlock);

            yield return null;
        }

        if (died)
        {
            rb.isKinematic = false;
            rb.AddForce(-transform.forward * 2f);
        }
    }
}