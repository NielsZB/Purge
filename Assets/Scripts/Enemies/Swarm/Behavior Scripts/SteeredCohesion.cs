using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Cohesion")]
public class SteeredCohesion : SwarmBehavior
{
    Vector3 currentVelocity;

    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm flock)
    {
        if (context.Count == 0)
            return Vector3.zero;

        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //create offset from agent position
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }

}
