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
    public bool ReadyToAttack { get; private set; }
    public bool TargetIsAlive { get; private set; }

    enum waitTypes
    {

    }


    public void MoveTowards(Vector3 target)
    {
        agent.destination = target;
    }

    public void Wait()
    {
        //StartCoroutine(Waiting());
    }

    public void Attack()
    {

    }

    IEnumerator Waiting(Vector2 waitRange)
    {
        ReadyToAttack = false;
        float waitTime = Random.Range(waitRange.x, waitRange.y);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / waitTime;

            yield return null;
        }
        ReadyToAttack = true;
    }
}
