using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    Swarm agentSwarm;
    public Swarm AgentSwarm { get { return agentSwarm; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    public Vector3 Position { get; private set; }
    public Vector3 Forward { get; private set; }

    public Transform Target { get; private set; }
    public bool hasTarget { get { return Target != null; } }
    public void Initialize(Swarm swarm)
    {
        agentCollider = GetComponent<Collider>();
        agentSwarm = swarm;
        Position = transform.position;
        Forward = transform.forward;
    }

    public void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity;
        Position = transform.position;
        Forward = transform.forward;
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
