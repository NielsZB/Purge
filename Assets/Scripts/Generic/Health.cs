using UnityEngine;

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

        //Debug.Log($"{name} was damaged by {amount} and has {CurrentHealth} left!", gameObject);
    }

    public void SetVulnerability(bool value)
    {
        isVulnerable = value;
    }
}