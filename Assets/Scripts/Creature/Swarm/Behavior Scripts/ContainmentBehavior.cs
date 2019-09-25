using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Containment")]
public class ContainmentBehavior : SwarmBehavior
{

    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField] float radius = 15f;

    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {
        Vector3 offsetFromCenter = center - agent.transform.position;

        float distanceToCenter = offsetFromCenter.magnitude / radius;

        if(distanceToCenter < 0.75f)
        {
            return Vector3.zero;
        }

        return offsetFromCenter * distanceToCenter * distanceToCenter;
    }
}
