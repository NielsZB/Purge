using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PTargeting : MonoBehaviour
{
    [SerializeField] Transform Indicator;
    [SerializeField] float radius = 10;
    [SerializeField] float updateRadiusMultiplier = 0.9f;
    [SerializeField] LayerMask mask = new LayerMask();
    [Space(10f)]
    [SerializeField] bool showDebug = false;

    public bool IsEnabled { get; private set; }

    public Transform Target { get; private set; }
    EHealth targetHealth;

    List<Transform> targets = new List<Transform>();
    Collider[] colliders = new Collider[10];

    Transform cameraTransform;

    public void GetTarget()
    {
        Vector3 direction = new Vector3(transform.forward.x, transform.forward.z);
        Target = GetTargetNearestDirection(direction);

        if (Target != null)
        {
            targetHealth = Target.GetComponent<EHealth>();
        }
    }
    public void StopTargeting()
    {
        Target = null;
        targets.Clear();
        IsEnabled = false;
    }

    public void GetTarget(Vector2 input)
    {
        Target = GetTargetNearestDirection(NormalizedCameraCorrectedInput(input));

    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        Indicator.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (IsEnabled)
        {
            if (targets.Count > 0)
            {
                if (targetHealth != null)
                {
                    if (targetHealth.IsDead)
                    {
                        targets.Remove(Target);
                        GetTarget();
                    }
                }

                Vector3 position = transform.position;
                for (int i = 0; i < targets.Count; i++)
                {
                    float distanceSqr = (targets[i].position - position).sqrMagnitude;

                    if (distanceSqr > radius * radius)
                    {
                        if (targets[i] == Target)
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
              
                IsEnabled = false;
            }
        }
    }

    private void Update()
    {
        if (Target != null)
        {
            Indicator.gameObject.SetActive(true);
            Indicator.position = Target.position + (Vector3.up * 1.1f);
        }
        else
        {
            Indicator.gameObject.SetActive(false);
        }
    }

    void UpdateTargetList()
    {
        int numberOfTargets = Physics.OverlapSphereNonAlloc(transform.position, radius * updateRadiusMultiplier, colliders, mask);

        targets.Clear();

        if (numberOfTargets > 0)
        {
            IsEnabled = true;
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets.Add(colliders[i].transform);
            }
        }
        else
        {
            Target = null;
            IsEnabled = false;
        }
    }

    Transform GetTargetNearestDirection(Vector3 direction)
    {
        direction.y = 0;
        UpdateTargetList();

        if (IsEnabled)
        {
            Transform bestTarget = null;
            float bestDirection = -1f;
            Vector3 position = transform.position;
            direction = direction.normalized;
            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 iDirection = (targets[i].position - position).normalized;

                float dotProduct = Vector3.Dot(direction, iDirection);

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

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius * updateRadiusMultiplier);
            if (targets.Count > 0)
            {
                Vector3 position = transform.position;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == Target)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.grey;
                    }
                    Gizmos.DrawLine(position, targets[i].position);
                }
            }
        }
    }
}