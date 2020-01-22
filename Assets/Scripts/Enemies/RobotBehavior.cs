using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotBehavior : MonoBehaviour
{
    public bool IsActive { get; private set; }
    public bool IsWaiting { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool TrackingWhileAttacking { get; private set; }
    public bool HasTarget { get { return target != null; } }

    bool inRangeOfTarget
    {
        get
        {
            if (agent == null)
            {
                return false;
            }
            if ((transform.position - target.position).sqrMagnitude <= agent.stoppingDistance * agent.stoppingDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    float normalizedSpeed
    {
        get
        {
            return agent.velocity.magnitude.Remap01(0, agent.speed);
        }
    }

    Transform target;
    NavMeshAgent agent;
    Animator animator;
    EnemyHealth health;
    Transform spawnpoint;

    EnemyManager manager;
    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponentInParent<EnemyHealth>();
    }

    private void Update()
    {
        if (IsActive && health.IsAlive)
        {
            if (TrackingWhileAttacking && HasTarget)
            {
                RotateTowardsTarget();
            }

            if (IsWaiting)
                return;

            animator.SetFloat("Movement", normalizedSpeed);

            if (HasTarget)
            {
                if (inRangeOfTarget)
                {
                    if (!IsAttacking)
                    {
                        Attack();
                    }


                }
                else
                {
                    MoveTowards(target.position);
                }
            }
        }
    }

    public void Activate()
    {
        IsActive = true;
        if (manager != null)
        {
            manager.EnableSpawnpoint(spawnpoint);
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Wait()
    {
        agent.isStopped = true;
        IsWaiting = true;
    }

    public void Wait(float duration)
    {
        Wait();
        StartCoroutine(Waiting(duration));
    }

    public void RotateTowardsTarget()
    {
        agent.transform.rotation = Quaternion.Slerp(
            agent.transform.rotation,
            Quaternion.LookRotation(target.position - transform.position),
            5f * Time.deltaTime);
    }

    public void StopTrackingWhileAttacking()
    {
        TrackingWhileAttacking = false;
    }
    public void StopWaiting()
    {
        IsWaiting = false;
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }

    public void MoveTowards(Vector3 position)
    {
        IsMoving = true;
        agent.destination = position;
    }

    public void Attack()
    {

        int attackRandomizer = Random.Range(0, 10);

        if (attackRandomizer <= 3)
        {
            animator.SetTrigger("Shock");
        }
        else
        {
            TrackingWhileAttacking = true;
            animator.SetTrigger("Attack");
        }
        Wait();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void EnableStun()
    {
        health.IsStunable = true;
    }

    public void DisableStun()
    {
        health.IsStunable = false;
    }
    IEnumerator Waiting(float duration)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            yield return null;
        }
        IsWaiting = false;
        IsAttacking = false;
    }

    public void InitializeRobot(EnemyManager manager)
    {
        this.manager = manager;
    }

    public void SetSpawnpoint(Transform point)
    {
        spawnpoint = point;
    }
}
