using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Alignment")]
public class AlignmentBehavior : SwarmBehavior
{
    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {
        if (context.Count == 0)
            return agent.transform.forward;

        Vector3 alignmentAverage = Vector3.zero;
        foreach (Transform item in context)
        {
            alignmentAverage += item.forward;
        }

        alignmentAverage /= context.Count;

        return alignmentAverage;
    }
}
