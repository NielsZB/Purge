using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public Agent agentPrefab;
    List<Agent> agents = new List<Agent>();
    public SwarmBehavior behavior;
    public float startingCount;
    public float driveFactor;
    public float maxSpeed;
    public float neighborRadius;
    public float avoidanceRadius;

    float sqrMaxSpeed;
    float sqrNeighborRadius;
    float sqrAvoidanceRadius;
    public float SqrAvoidanceRadius { get { return sqrAvoidanceRadius; } }

    const float agentDensity = 0.08f;
    private void Start()
    {
        sqrMaxSpeed = maxSpeed * maxSpeed;
        sqrNeighborRadius = neighborRadius * neighborRadius;
        sqrAvoidanceRadius = avoidanceRadius * avoidanceRadius;

        for (int i = 0; i < startingCount; i++)
        {
            Agent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * agentDensity,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);

            Vector3 p = newAgent.transform.position;
            //p.y = transform.position.y;
            newAgent.transform.position = p;

            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            List<Transform> context = GetNearbyObjects(agents[i]);

            Vector3 move = behavior.CalculateMove(agents[i], context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > sqrMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agents[i].Move(move);
        }
    }

    List<Transform> GetNearbyObjects(Agent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    public void SetSwarmTarget(Transform target)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].SetTarget(target);
        }
    }
}