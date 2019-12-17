using System.Collections;
using UnityEngine;

public class EHealth : Health
{
    [SerializeField] float siphonAmount;
    [SerializeField] float siphonDuration;
    [SerializeField] Gradient siphonGradient;
    [SerializeField] Rigidbody swordRB;
    public float SiphonAmount { get { return siphonAmount; } }
    public bool Siphonable { get; private set; }


    EnemyBehavior behavior;
    readonly bool canSiphon = false;

    protected override void Awake()
    {
        base.Awake();
        behavior = GetComponent<EnemyBehavior>();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        StartCoroutine(ShowImpact(CurrentHealth <= 0));

    }

    IEnumerator ShowImpact(bool died)
    {
        float t = 0f;
        float duration;
        Gradient gradient;

        if (died)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            IsDead = true;
            duration = 0.5f;
            gradient = deathGradient;

            behavior.Disable();

            if (canSiphon)
            {
                Siphonable = true;

                while (t < 1)
                {
                    t += Time.deltaTime / siphonDuration;
                    render.GetPropertyBlock(materialPropertyBlock);
                    materialPropertyBlock.SetColor("_BaseColor", siphonGradient.Evaluate(t));
                    render.SetPropertyBlock(materialPropertyBlock);
                    yield return null;
                }

                Siphonable = false;
            }
        }
        else
        {
            duration = 0.25f;
            gradient = hitGradient;
        }

        t = 0;
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
            swordRB.GetComponent<Collider>().isTrigger = false;
            swordRB.isKinematic = false;
            swordRB.AddForceAtPosition(swordRB.position - transform.position * 5f, transform.position);
            rb.isKinematic = false;
            rb.AddForce(-transform.forward * 5f);
        }
    }
}
