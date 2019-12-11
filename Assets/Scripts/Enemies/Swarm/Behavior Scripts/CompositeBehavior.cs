using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Composite")]
public class CompositeBehavior : SwarmBehavior
{
    [System.Serializable]
   public struct BehaviorData
    {
        public SwarmBehavior type;
        public float weight;
    }

    public BehaviorData[] behaviors;

    public override Vector3 CalculateMove(Agent agent, List<Transform> context, Swarm swarm)
    {
        if(behaviors.Length == 0)
        {
            Debug.Log("No Behaviors for the swarm!");
            return Vector3.zero;
        }

        Vector3 move = Vector3.zero;

        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].type.CalculateMove(agent,context,swarm) * behaviors[i].weight;

            if(partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude > behaviors[i].weight * behaviors[i].weight)
                {
                    partialMove.Normalize();
                    partialMove *= behaviors[i].weight;
                }

                move += partialMove;
            }
        }

        return move;
    }
}
