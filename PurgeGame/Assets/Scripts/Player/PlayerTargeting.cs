using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
public class PlayerTargeting : MonoBehaviour
{
    #region Serialized Variables

    [SerializeField] float radius;
    [SerializeField] LayerMask targetMask;
    [SerializeField] bool showDebug;

    #endregion

    #region public Variables
    public bool IsTargeting { get; private set; }

    public Transform Target { get; private set; }

    #endregion

    #region Private Variables

    List<Transform> targets = new List<Transform>();
    Collider[] colliders = new Collider[10];

    Transform cameraTransform;
    PlayerCamera cameraModule;

    #endregion

    #region Public Methods

    public void ToggleTargeting()
    {
        SetTargeting(!IsTargeting);
    }
    public void SetTargeting(bool _state)
    {
        IsTargeting = _state;

        if (!IsTargeting)
        {
            Target = null;
        }

        cameraModule.EnableTargetingCamera(Target != null);
    }
    public void PickTarget()
    {
        Target = GetClosestTarget();
    }
    public void PickTarget(Vector2 _input)
    {
        Vector3 input;

        if (_input != Vector2.zero)
        {
            Vector3 correctedVertical = _input.y * cameraTransform.forward;
            Vector3 correctedHorizontal = _input.x * cameraTransform.right;

            input = (correctedVertical + correctedHorizontal).normalized;

            Vector3 direction = new Vector3(input.x, 0, input.z);

            if (showDebug)
            {
                Debug.DrawRay(transform.position, -direction * 10f, Color.cyan);
            }

            Target = GetTargetNearestInDirection(direction);
        }
        else
        {
            Target = GetTargetNearestInDirection(transform.forward);

        }
    }

    #endregion

    #region Private Methods
    void GetTargets()
    {
        int numberOfTargets = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, targetMask);

        targets.Clear();

        if (numberOfTargets > 0)
        {
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets.Add(colliders[i].transform);
            }
        }
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        cameraModule = GetComponent<PlayerCamera>();
    }

    Transform GetClosestTarget()
    {
        GetTargets();
        if (targets.Count > 0)
        {
            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            for (int i = 0; i < targets.Count; i++)
            {
                float distanceSqr = (targets[i].position - currentPosition).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestTarget = targets[i];
                }
            }
            return closestTarget;
        }
        else
        {
            return null;
        }
    }

    Transform GetTargetNearestInDirection(Vector3 _targetDirection)
    {
        GetTargets();
        if (targets.Count > 0)
        {
            Transform bestTarget = null;
            float bestDirection = -1f;
            Vector3 position = transform.position;
            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 direction = position - targets[i].position;


                float dotProduct = Vector3.Dot(_targetDirection.normalized, direction.normalized);

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

    private void FixedUpdate()
    {
        cameraModule.EnableTargetingCamera(Target != null);
        if (Target != null)
        {
            float distanceSqr = (Target.position - transform.position).sqrMagnitude;

            if (distanceSqr > radius * radius)
            {
                Target = null;
                SetTargeting(false);
            }
        }
    }

    #endregion

    #region Debug Visualization

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (IsTargeting)
            {
                Gizmos.color = Color.cyan;
            }
            else
            {
                Gizmos.color = Color.black;
            }
            Gizmos.DrawWireSphere(transform.position, radius);

            if (Target != null)
            {
                Gizmos.DrawLine(transform.position, Target.position);
            }
        }
    }

    #endregion
}
