using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Targeting")]
public class TargetingBehavior : SwarmBehavior
{
    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {
        if (agent.hasTarget)
        {
            return (agent.Target.position - agent.transform.position).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
