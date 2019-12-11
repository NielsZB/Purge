using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Avoidance")]
public class AvoidanceBehavior : SwarmBehavior
{
    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {
        if (context.Count == 0)
            return Vector3.zero;

        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            if (item.CompareTag("Player"))
            {
                nAvoid++;
                avoidanceMove += (agent.transform.position - item.position);
            }
            else
            {
                if (Vector3.SqrMagnitude(item.position - agent.transform.position) < swarm.SqrAvoidanceRadius)
                {
                    nAvoid++;
                    avoidanceMove += (agent.transform.position - item.position);
                }
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove;
    }
}
