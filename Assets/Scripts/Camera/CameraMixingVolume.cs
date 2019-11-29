using Cinemachine;
using UnityEngine;

public class CameraMixingVolume : MonoBehaviour
{
    [System.Serializable]
    public struct Zone
    {
        public CinemachineVirtualCameraBase camera;
        [HideInInspector] public float weight;
        public Vector3 position;
        public float radius;
        public float mixWidth;
        public AnimationCurve mixingCurve;
    }

    [SerializeField] float smoothing = 0.15f;

    public Zone[] zones;


    public Transform Target { get; private set; }
    public bool HasTarget { get { return Target != null; } }

    CameraMixingManager manager;

    BlendCamera[] blendCameras;
    BlendCamera exitBlendCamera;
    private void Start()
    {
        manager = FindObjectOfType<CameraMixingManager>();
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            Debug.LogError(name + " needs a collider to function!", this);
        }

        blendCameras = new BlendCamera[zones.Length];

        for (int i = 0; i < blendCameras.Length; i++)
        {

            BlendCamera blendCamera = blendCameras[i];
            if (zones[i].camera == null)
            {
                Debug.LogError("Zone " + i + " needs a virtual camera to function correctly!", this);
            }
            else
            {
                blendCamera.camera = zones[i].camera;
            }

            blendCamera.weight = zones[i].weight;
            blendCamera.originalParent = blendCamera.camera.transform.parent;

            blendCameras[i] = blendCamera;
        }
    }

    void Update()
    {
        if (HasTarget)
        {
            UpdateWeights();
        }
        else
        {
            if (exitBlendCamera.camera != null)
            {
                float compoundedWeight = 0f;

                for (int i = 0; i < blendCameras.Length; i++)
                {
                    if (blendCameras[i].camera != exitBlendCamera.camera)
                    {
                        compoundedWeight += blendCameras[i].weight;
                    }
                }

                if (compoundedWeight > 0.01f)
                {
                    for (int i = 0; i < zones.Length; i++)
                    {
                        if (blendCameras[i].camera != exitBlendCamera.camera)
                        {
                            blendCameras[i].weight = Mathf.SmoothStep(blendCameras[i].weight, 0f, smoothing);
                        }
                    }
                    manager.UpdateWeight(blendCameras);
                }
                else
                {
                    manager.Remove(blendCameras);
                    manager.Add(exitBlendCamera);
                    exitBlendCamera.camera = null;
                }
            }
        }

    }

    private void UpdateWeights()
    {
        Vector3 position = Target.position;

        for (int i = 0; i < zones.Length; i++)
        {
            Zone zone = zones[i];

            float distance = (zone.position - position).magnitude;

            if (distance < zone.radius + zone.mixWidth)
            {
                if (distance < zone.radius)
                {
                    zone.weight = 1f;
                }
                else
                {
                    zone.weight = distance.Remap(zone.radius + zone.mixWidth, zone.radius, zone.mixingCurve);
                }
            }
            else
            {
                zone.weight = 0f;
            }

            blendCameras[i].weight = Mathf.SmoothStep(blendCameras[i].weight, zone.weight, smoothing);
        }

        manager.UpdateWeight(blendCameras);
    }

    private void OnTriggerEnter(Collider other)
    {
        Target = other.transform;
        manager.Add(blendCameras);
    }

    private void OnTriggerExit(Collider other)
    {
        float highestWeight = 0f;

        for (int i = 0; i < blendCameras.Length; i++)
        {
            if (blendCameras[i].weight > highestWeight)
            {
                exitBlendCamera = blendCameras[i];
                highestWeight = blendCameras[i].weight;
            }
        }
        Target = null;
    }
}
