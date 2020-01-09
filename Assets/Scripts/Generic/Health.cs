using UnityEngine;
using System.Collections;
public class Health : MonoBehaviour
{
    [SerializeField] bool isVulnerable = true;
    [SerializeField] protected float health = 10f;
    [SerializeField] protected Gradient hitGradient;
    [SerializeField] protected Gradient deathGradient;
    [SerializeField] protected Renderer render;


    public float CurrentHealth { get; protected set; }
    public bool IsDead { get; protected set; } = false;
    public bool IsVulnerable { get { return IsVulnerable; } }

    protected MaterialPropertyBlock materialPropertyBlock;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        CurrentHealth = health;
        materialPropertyBlock = new MaterialPropertyBlock();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void TakeDamage(float amount)
    {
        if (!isVulnerable)
            return;

        CurrentHealth -= amount;

        if(CurrentHealth <= 0)
        {
            IsDead = true;
        }

        StartCoroutine(ShowImpact());
    }

    protected virtual IEnumerator ShowImpact()
    {
        float t = 0;

        float duration;
        Gradient gradient = new Gradient();

        if(IsDead)
        {
            duration = 0.15f;
            gradient = deathGradient;
        }
        else
        {
            duration = 0.25f;
            gradient = hitGradient;
        }

        while(t < 1)
        {
            t += Time.deltaTime / duration;

            render.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_baseColor", gradient.Evaluate(t));
            render.SetPropertyBlock(materialPropertyBlock);

            yield return null;
        }

        if(IsDead)
        {
            rb.isKinematic = false;
            rb.AddForce(-transform.forward * 2f);
        }
    }

    public void SetVulnerability(bool value)
    {
        isVulnerable = value;
    }
}