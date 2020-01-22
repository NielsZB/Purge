using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;

    public bool IsAlive { get; private set; } = true;
    public float CurrentHealth { get; private set; }

    public bool IsStunned { get; private set; }

    public bool IsStunable;
    Animator animator;
    NavMeshAgent agent;

    EnemyManager manager;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        CurrentHealth = health;
    }
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
        {
            IsAlive = false;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren <RobotBehavior>().enabled = false;
            animator.SetTrigger("Killed");
            manager.RemoveRobot();
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
    }
}
