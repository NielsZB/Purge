using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField, MinMaxSlider(0f, 5f)] Vector2 attackWaitTimeRange;
    NavMeshAgent agent;
    public bool InAttackRange { get; private set; }
    public bool ReadyToAttack { get; private set; } = true;
    public bool TargetIsAlive { get; private set; }
    public bool hasAttacked { get; private set; } = false;


    PlayerHealth player;
    Animator animator;
    Damager damager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damager = GetComponentInChildren<Damager>(true);
        player = FindObjectOfType<PlayerHealth>();
        MoveTowards(player.transform.position);
    }

    public void Disable()
    {
        agent.enabled = false;
        animator.enabled = false;
    }
    private void Update()
    {
        if (!player.IsAlive && agent.enabled)
        {
            if (hasAttacked)
            {
                agent.isStopped = true;
            }
            else
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (ReadyToAttack)
                    {
                        Attack();
                    }
                }
                else
                {
                    MoveTowards(player.transform.position);
                }
            }

            if (!ReadyToAttack)
            {
                SetDirectionRotation(player.transform.position - transform.position);
            }
        }
    }

    void SetDirectionRotation(Vector3 point)
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(point, Vector3.up),
            30f);

    }
    void MoveTowards(Vector3 target)
    {
        agent.destination = target;
    }

    public void Wait()
    {
        StartCoroutine(Waiting());
    }

    public void EnableDamage()
    {
        damager.CanDamage = true;
    }
    public void DisableDamage()
    {
        damager.CanDamage = false;
    }
    void Attack()
    {
        if (hasAttacked)
            return;

        hasAttacked = true;
        int attackType = Random.Range(0, 2);
        animator.SetInteger("AttackType", attackType);
        animator.SetTrigger("Attack");
    }

    IEnumerator Waiting()
    {
        ReadyToAttack = false;
        float waitTime = Random.Range(attackWaitTimeRange.x, attackWaitTimeRange.y);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / waitTime;

            yield return null;
        }
        hasAttacked = false;
        ReadyToAttack = true;
        MoveTowards(player.transform.position);
        agent.isStopped = false;
    }
}
