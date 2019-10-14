using Cinemachine;
using UnityEngine;

public class CameraMixingVolume : MonoBehaviour
{
    [System.Serializable]
    public struct Zone
    {
        public CinemachineVirtualCameraBase camera;
        [HideInInspector] public float cameraWeight;
        public float weight;
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

            blendCamera.weight = zones[i].cameraWeight * zones[i].weight;
            blendCamera.originalParent = blendCamera.camera.transform.parent;

            blendCameras[i] = blendCamera;
        }
    }

    private void Update()
    {
        if (HasTarget)
        {
            UpdateWeights();

            for (int i = 0; i < zones.Length; i++)
            {
                blendCameras[i].weight = Mathf.SmoothStep(blendCameras[i].weight, zones[i].cameraWeight, smoothing);
            }

            manager.UpdateWeight(blendCameras);
        }
    }
    void UpdateWeights()
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
                    zone.cameraWeight = 1f * zone.weight;
                }
                else
                {
                    zone.cameraWeight = distance.Remap(zone.radius + zone.mixWidth, zone.radius, zone.mixingCurve) * zone.weight;
                }
            }
            else
            {
                zone.cameraWeight = 0f;
            }

            zones[i] = zone;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Target = other.transform;
        manager.Add(blendCameras);
    }

    private void OnTriggerExit(Collider other)
    {

        float highestWeight = 0f;
        BlendCamera activeCamera = new BlendCamera();

        for (int i = 0; i < blendCameras.Length; i++)
        {
            if (blendCameras[i].weight > highestWeight)
            {
                activeCamera = blendCameras[i];
                highestWeight = blendCameras[i].weight;
            }
        }
        manager.Remove(blendCameras);
        manager.Add(activeCamera);
        Target = null;
    }
}
