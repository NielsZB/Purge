using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] Gradient hitGradient;
    public bool IsAlive { get; private set; } = true;
    public float CurrentHealth { get; private set; }

    public bool IsStunned { get; private set; }

    public bool IsStunable;
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] EnemyManager manager;
    bool managed = false;
    MaterialPropertyBlock propertyBlock;
    Renderer[] render;
    RobotBehavior behavior;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        propertyBlock = new MaterialPropertyBlock();
        render = GetComponentsInChildren<Renderer>();
        behavior = GetComponentInChildren<RobotBehavior>();
        CurrentHealth = health;
    }

    public void TakeDamage(float amount, Transform enemy = null)
    {
        CurrentHealth -= amount;
        StartCoroutine(Hit());

        if (!managed)
        {
            if (enemy != null)
            {
                behavior.SetTarget(enemy);
            }
        }

        if (CurrentHealth < 0)
        {
            IsAlive = false;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<RobotBehavior>().enabled = false;
            animator.SetTrigger("Killed");
            if (manager != null)
            {
                manager.RemoveRobot();
            }

            if (!managed)
            {
                if (manager != null)
                {
                    manager.TriggerEncounter();
                }
            }
        }
    }

    public void Stun()
    {
        if (IsStunable)
        {

            IsStunned = true;
            agent.isStopped = true;
            animator.ResetTrigger("attack");
            animator.SetTrigger("Hit");
        }
    }


    public void ResetStun()
    {
        IsStunned = false;
    }

    public void InitializeRobot(EnemyManager manager)
    {
        this.manager = manager;
        managed = true;
    }

    IEnumerator Hit()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 0.25f;
            for (int i = 0; i < render.Length; i++)
            {
                render[i].GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_EmissiveColor", hitGradient.Evaluate(t));
                render[i].SetPropertyBlock(propertyBlock);
            }

            yield return null;
        }
    }
}
