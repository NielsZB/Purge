using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class WeightVolume : MonoBehaviour
{
    public enum Type
    {
        Transition,
        Focus,
        Curve
    }

    enum ColliderType
    {
        Sphere,
        Box,
        Mesh
    }

    [SerializeField, Tooltip(
        "TRANSITION blends through the collider on the z axis. " +
        "FOCUS blends from the edge to the center (box Collider is on the z axis). " +
        "CURVE blends using a cinemachinepath."),
        ValidateInput("HAS_PATH", "Curve needs a CinemachinePath or CinemachineSmoothPath!")]
    Type type = Type.Transition;
    [SerializeField, Tooltip("How the blend is weighted over the blend.")]
    AnimationCurve blendCurve = null;
    [SerializeField, Tooltip("How smooth the weight is changed.")]
    float smoothing = 0.25f;
    [SerializeField]
    float outsideSmoothing = 0.5f;

    public float Weight { get; private set; }
    public Type VolumeType { get { return type; } }
    public bool HasTarget { get { return target != null; } }
    
    ColliderType colliderType;
    bool targetInVolume;
    float blendRange;

    CinemachinePathBase path;
    Transform target;

    bool HAS_PATH(Type type)
    {
        path = GetComponent<CinemachinePathBase>();
        if (type == Type.Curve && path == null)
        {
            return false;
        }
        else
            return true;
    }
    private void Start()
    {
        Collider collider = GetComponent<Collider>();


        if (collider != null)
        {
            collider.isTrigger = true;

            if (collider.GetType() == typeof(BoxCollider))
            {
                colliderType = ColliderType.Box;
                blendRange = GetComponent<BoxCollider>().size.z / 2;
            }
            else if (collider.GetType() == typeof(SphereCollider))
            {
                colliderType = ColliderType.Sphere;
                blendRange = GetComponent<SphereCollider>().radius;

                if (type == Type.Transition)
                {
                    Debug.LogError("Transition volume types only works with BoxColliders", this);
                    Debug.Break();
                }
            }
            else if (collider.GetType() == typeof(MeshCollider))
            {
                colliderType = ColliderType.Mesh;

                if (type != Type.Curve)
                {
                    Debug.LogError("MeshColliders only work with curve volume types.", this);
                    Debug.Break();
                }
            }

            if (type == Type.Curve)
            {
                path = GetComponent<CinemachinePathBase>();

                if (path != null)
                {
                    blendRange = path.MaxPos;
                }
                else
                {
                    Debug.LogError("Curve volume types needs a CinemachinePath or CinemachineSmoothPath Component.", this);
                    Debug.Break();
                }
            }
        }
        else
        {
            Debug.LogError("WeightVolume needs a collider. Add an appropriate collider.", this);
            Debug.Break();
        }
    }

    private void FixedUpdate()
    {
        if (targetInVolume)
        {
            Weight = Mathf.SmoothStep(Weight, GetWeight(), smoothing);
        }
        else
        {
            if (HasTarget)
            {
                Weight = Weight > 0.5f ? Mathf.SmoothStep(Weight, 1, outsideSmoothing) : Mathf.SmoothStep(Weight, 0, outsideSmoothing);

                if (Weight < 0.001f)
                {
                    Weight = 0;
                    target = null;
                }
                else if (Weight > 0.999f)
                {
                    Weight = 1f;
                    target = null;
                }
            }
        }
    }

    public float GetWeight()
    {
        float distance;

        if (type == Type.Transition)
        {
            distance = transform.InverseTransformPoint(target.position).z;
            distance = blendCurve.Evaluate(distance.Remap01(-blendRange, blendRange));
        }
        else if (type == Type.Focus)
        {
            if (colliderType == ColliderType.Box)
            {
                distance = Mathf.Abs(transform.InverseTransformPoint(target.position).z);
            }
            else
            {
                distance = (transform.position - target.position).magnitude;
            }

            distance = blendCurve.Evaluate(distance.Remap01(blendRange, 0f));
        }
        else
        {
            distance = path.FindClosestPoint(target.position, 0, 1, 10);

            distance = blendCurve.Evaluate(distance.Remap01(0f, blendRange));
        }

        return distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        targetInVolume = true;
        target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        targetInVolume = false;
    }
}
