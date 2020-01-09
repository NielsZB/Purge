using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public Transform Target { get; private set; }

    public bool HasTarget { get { return Target != null; } }

    [SerializeField] float radius = 10f;
    [SerializeField] float updateRadiusMultiplier = 0.9f;
    [SerializeField] LayerMask mask = new LayerMask();
    [SerializeField] bool showDebug = false;


    public bool isTargeting { get; private set; }

    List<Transform> targets = new List<Transform>();
    Collider[] colliders = new Collider[20];

    Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (isTargeting)
        {
            if (targets.Count > 0)
            {
                Vector3 position = transform.position;

                for (int i = 0; i < targets.Count; i++)
                {
                    float distanceSqr = (targets[i].position - position).sqrMagnitude;

                    if(distanceSqr > radius * radius)
                    {
                        if(targets[i] == Target)
                        {
                            GetTarget();
                        }
                        else
                        {
                            targets.Remove(targets[i]);
                        }
                    }
                }
            }
            else
            {
                isTargeting = false;
            }
        }
    }

    void UpdateTargetList()
    {
        int numberOfTargets = Physics.OverlapSphereNonAlloc(transform.position, radius * updateRadiusMultiplier, colliders, mask);

        targets.Clear();

        if (numberOfTargets > 0)
        {
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets.Add(colliders[i].transform);
            }
        }
        else
        {
            Target = null;
            isTargeting = false;
        }
    }

    Transform GetTargetInDirection(Vector3 direction)
    {

        UpdateTargetList();

        if (isTargeting)
        {
            direction.y = 0;
            direction.Normalize();

            Transform bestTarget = null;
            float bestDirection = -1f;
            Vector3 position = transform.position;

            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 targetDirection = (targets[i].position - position).normalized;

                float dotProduct = Vector3.Dot(direction, targetDirection);

                if (dotProduct > bestDirection)
                {
                    bestTarget = targets[i];
                    bestDirection = dotProduct;
                }
            }

            return bestTarget;
        }
        else
        {
            return null;
        }
    }

    Vector3 NormalizedCameraCorrectedInput(Vector2 input)
    {
        Vector3 correctedVertical = input.y * cameraTransform.forward;
        Vector3 correctedHorizontal = input.x * cameraTransform.right;

        return (correctedVertical + correctedHorizontal).normalized;
    }

    public void GetTarget()
    {
        Vector3 direction = new Vector3(transform.forward.x, transform.forward.z);
        Target = GetTargetInDirection(direction);
    }

    public void GetTarget(Vector2 input)
    {
        Target = GetTargetInDirection(NormalizedCameraCorrectedInput(input));
    }

    public void StopTargeting()
    {
        Target = null;
        targets.Clear();
        isTargeting = false;
    }
}
