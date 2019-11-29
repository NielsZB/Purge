using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineMixingCamera))]
public class MixingVolume : MonoBehaviour
{
    public enum Type
    {
        Transition,
        Focus,
        Curve,
        Composite
    }

    enum ColliderType
    {
        sphere,
        box,
        mesh
    }

    public Type type;
    [SerializeField] AnimationCurve blendCurve;
    [SerializeField] float smoothing;


    public float Weight { get; private set; }
    public bool HasTarget { get { return target != null; } }

    CinemachineVirtualCameraBase ACamera;
    CinemachineVirtualCameraBase BCamera;
    CinemachineMixingCamera mixer;
    CinemachineBrain brain;
    CinemachinePathBase path;
    Transform target;
    ColliderType colliderType;

    bool targetInVolume;
    float blendRange;


    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        mixer = GetComponent<CinemachineMixingCamera>();
        BCamera = mixer.ChildCameras[0];
        mixer.enabled = false;

        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            collider.isTrigger = true;

            if (collider.GetType() == typeof(SphereCollider))
            {
                if (type != Type.Focus)
                {
                    Debug.LogError("Type is set to " + type.ToString() + " and has a Sphere Collider attached. Sphere Colliders only work with focus volumes.", this);
                    Debug.Break();
                }

                colliderType = ColliderType.sphere;
                blendRange = GetComponent<SphereCollider>().radius;
            }
            else if (collider.GetType() == typeof(BoxCollider))
            {
                colliderType = ColliderType.box;
                blendRange = GetComponent<BoxCollider>().size.z / 2;
            }
            else if (collider.GetType() == typeof(MeshCollider))
            {
                if (type != Type.Composite)
                {
                    Debug.LogError("Type is set to " + type.ToString() + " and has a Mesh Colliders attached. Mesh Colliders only work with composite volumes.", this);
                    Debug.Break();
                }

                colliderType = ColliderType.mesh;
            }
        }
        else
        {
            Debug.LogError(name + " needs a collider. The Mixing Volume type is set to " + type + ". Add an appropriate Collider.", this);
            Debug.Break();
        }

        if (type == Type.Curve)
        {
            path = GetComponent<CinemachinePathBase>();

            if (path == null)
            {
                Debug.LogError("The MixingVolume type is set to Curve. Add an CinemachinePath component.", this);
                Debug.Break();
            }
            else
            {
                blendRange = path.MaxPos;
            }
        }

    }


    private void FixedUpdate()
    {
        if (targetInVolume)
        {
            if (type == Type.Transition)
            {
                float distance = transform.InverseTransformPoint(target.position).z;

                distance = blendCurve.Evaluate(distance.Remap01(-blendRange, blendRange));

                Weight = Mathf.SmoothStep(Weight, distance, smoothing);

                mixer.SetWeight(ACamera, 1 - Weight);
                mixer.SetWeight(BCamera, Weight);

            }
            else if (type == Type.Focus)
            {
                float distance;

                if (colliderType == ColliderType.sphere)
                {
                    distance = (transform.position - target.position).magnitude;
                }
                else
                {
                    distance = Mathf.Abs(transform.InverseTransformPoint(target.position).z);

                    distance.Clamped(0f, blendRange);
                }

                distance = blendCurve.Evaluate(distance.Remap01(blendRange, 0f));

                Weight = Mathf.SmoothStep(Weight, distance, smoothing);

                mixer.SetWeight(ACamera, 1 - Weight);
                mixer.SetWeight(BCamera, Weight);
            }
            else if (type == Type.Curve)
            {
                float distance = path.FindClosestPoint(target.position, 0, -1, 10);

                Debug.Log(blendRange);
                distance = blendCurve.Evaluate(distance.Remap01(0f, blendRange));

                Weight = Mathf.SmoothStep(Weight, distance, smoothing);

                mixer.SetWeight(ACamera, 1 - Weight);
                mixer.SetWeight(BCamera, Weight);
            }
            else if (type == Type.Composite)
            {

            }
        }
        else
        {
            if (HasTarget)
            {
                if (type == Type.Transition || type == Type.Curve)
                {
                    Weight = Weight > 0.5f ? Mathf.SmoothStep(Weight, 1, smoothing) : Mathf.SmoothStep(Weight, 0, smoothing);

                    mixer.SetWeight(ACamera, 1 - Weight);
                    mixer.SetWeight(BCamera, Weight);

                    if (Weight < 0.001f)
                    {
                        Weight = 0f;
                        ACamera.transform.SetParent(null, true);
                        mixer.enabled = false;
                        target = null;
                    }
                    else if (Weight > 0.999f)
                    {
                        Weight = 1f;
                        BCamera.transform.SetParent(null, true);
                        mixer.enabled = false;
                        target = null;
                    }


                }
                else if (type == Type.Focus)
                {
                    Weight = Mathf.SmoothStep(Weight, 0f, smoothing);

                    mixer.SetWeight(ACamera, 1 - Weight);
                    mixer.SetWeight(BCamera, Weight);

                    if (Weight < 0.01f)
                    {
                        ACamera.transform.SetParent(null, true);

                        mixer.enabled = false;
                        target = null;
                    }
                }
                else if (type == Type.Composite)
                {
                }
            }
        }
    }

    /* private void Update()
    {
        if (type == Type.Transition)
        {
            if (HasTarget)
            {

                float distance = transform.InverseTransformPoint(target.position).z;
                float distanceRemapped = blendCurve.Evaluate(Mathf.Clamp01(distance.Remap(-halfDepth, halfDepth)));

                Weight = Mathf.SmoothStep(Weight, distanceRemapped, smoothing);

                mixer.SetWeight(BCamera, Weight);
                mixer.SetWeight(ACamera, 1 - Weight);
            }
            else
            {
                if (BCamera != null && ACamera != null)
                {

                    Weight = Weight > 0.5f ? Mathf.SmoothStep(Weight, 1, smoothing) : Mathf.SmoothStep(Weight, 0, smoothing);

                    mixer.SetWeight(BCamera, Weight);
                    mixer.SetWeight(ACamera, 1 - Weight);

                    if (Weight > 0.99f)
                    {
                        BCamera.transform.SetParent(null, true);
                        BCamera = null;
                        mixer.Priority = 0;
                        mixer.enabled = false;
                    }
                    else if (Weight < 0.01f)
                    {
                        ACamera.transform.SetParent(null, true);
                        ACamera = null;
                        mixer.Priority = 0;
                        mixer.enabled = false;
                    }
                }
            }
        }
        else if (type == Type.Focus)
        {
            if (HasTarget)
            {
                float distance;
                if (radius != 0f)
                {
                    distance = (transform.position - target.position).magnitude;

                    distance = blendCurve.Evaluate(Mathf.InverseLerp(radius, 0, distance));
                }
                else
                {
                    distance = Mathf.Abs(transform.InverseTransformPoint(target.position).z);

                    distance = distance > halfDepth ? halfDepth : distance;

                    distance = blendCurve.Evaluate(Mathf.InverseLerp(halfDepth, 0f, distance));
                }

                Weight = Mathf.SmoothStep(Weight, distance, smoothing);

                mixer.SetWeight(BCamera, Weight);
                mixer.SetWeight(ACamera, 1 - Weight);
            }
            else
            {
                if (Weight > 0.01f)
                {
                    Weight = Mathf.SmoothStep(Weight, 0f, smoothing);

                    mixer.SetWeight(BCamera, Weight);
                    mixer.SetWeight(ACamera, 1 - Weight);
                }
                else
                {
                    if (ACamera != null)
                    {
                        ACamera.transform.SetParent(null, true);
                        ACamera = null;
                        mixer.Priority = 0;
                        mixer.enabled = false;
                    }
                }
            }
        }
        else if (type == Type.Composite)
        {
        }
    }*/


    private void OnTriggerEnter(Collider other)
    {
        targetInVolume = true;
        target = other.transform;
        mixer.enabled = true;

        if (type == Type.Transition)
        {
            float distance = transform.InverseTransformPoint(target.position).z;

            distance = blendCurve.Evaluate(distance.Remap01(-blendRange, blendRange));

            if (distance > 0.5f)
            {
                if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera != mixer)
                {
                    BCamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                    BCamera.transform.SetParent(transform, true);
                }
            }
            else
            {
                if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera != mixer)
                {
                    ACamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                    ACamera.transform.SetParent(transform, true);
                }
            }
        }
        else if (type == Type.Focus)
        {
            if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera != mixer)
            {
                ACamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                ACamera.transform.SetParent(transform, true);
            }
        }
        else if (type == Type.Curve)
        {
            float distance = path.FindClosestPoint(target.position, 0, -1, 10);

            distance = blendCurve.Evaluate(distance.Remap01(0f, blendRange));

            if (distance > 0.5f)
            {
                if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera != mixer)
                {
                    BCamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                    BCamera.transform.SetParent(transform, true);
                }
            }
            else
            {
                if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera != mixer)
                {
                    ACamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                    ACamera.transform.SetParent(transform, true);
                }
            }
        }
        else if (type == Type.Composite)
        {

        }

        mixer.Priority = 99;

        var x = mixer.ChildCameras;

    }

    private void OnTriggerExit(Collider other)
    {
        targetInVolume = false;
    }
}
