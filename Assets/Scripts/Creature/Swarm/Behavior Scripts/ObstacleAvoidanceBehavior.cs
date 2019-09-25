using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Obstacle Avoidance")]
public class ObstacleAvoidanceBehavior : SwarmBehavior
{
    [SerializeField] float boundsRadius = 0.25f;
    [SerializeField] float avoidanceCheckDistance = 5f;
    [SerializeField] LayerMask obstacleMask = new LayerMask();

    [SerializeField] float smoothTime = 0.5f;

    Vector3 currentVelocity;
    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {

        Ray ObstacleCheckingRay = new Ray(agent.transform.position, agent.transform.forward);

        if (Physics.SphereCast(ObstacleCheckingRay, boundsRadius, avoidanceCheckDistance, obstacleMask))
        {
            Vector3[] rayDirections = AgentEnvironmentChecker.directions;

            for (int i = 0; i < rayDirections.Length; i++)
            {
                Vector3 dir = agent.transform.TransformDirection(rayDirections[i]);
                Ray ray = new Ray(agent.Position, dir);
                if (!Physics.SphereCast(ray, boundsRadius, avoidanceCheckDistance, obstacleMask))
                {
                    dir -= agent.transform.position;
                    dir = Vector3.SmoothDamp(agent.transform.forward, dir, ref currentVelocity, smoothTime);
                    Debug.DrawRay(agent.transform.position, dir * avoidanceCheckDistance, Color.green);
                    return dir;
                }
                else
                {
                    Debug.DrawRay(agent.transform.position, dir * avoidanceCheckDistance, Color.red);
                }
            }
            return agent.Forward;
        }
        else
        {
            return Vector3.zero;
        }
    }
}