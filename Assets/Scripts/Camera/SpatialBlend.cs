using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class SpatialBlend : MonoBehaviour
{
    public enum Type
    {
        Sphere,
        Box,
        Mesh
    }

    [System.Serializable]
    public struct Zone
    {
        public string name;
        public Type volumeType;
        public Vector3 point;
        public float radius;
        public float blendRadius;
        public float weight;
        public AnimationCurve blendCurve;
    }

    /// <summary>
    /// Should not be modified.
    /// </summary>
    public Zone[] zones;

    public float[] weights { get; private set; }

    public Transform Target { get; private set; }

    public bool HasTarget { get { return Target != null; } }

    private void Awake()
    {
        weights = new float[zones.Length];
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (HasTarget)
        {
            GetWeights();
        }
    }

    void GetWeights()
    {
        if (!HasTarget)
            return;

        if (zones.Length == 0)
            return;

        Vector3 position = Target.position;

        for (int i = 0; i < zones.Length; i++)
        {
            Zone zone = zones[i];
            float distance = (zone.point - position).magnitude;
            Debug.Log(i + "   " + distance);
            if (distance < zone.radius)
            {
                if (distance < zone.blendRadius)
                {
                    weights[i] = 1f * zone.weight;
                }
                else
                {
                    weights[i] = distance.Remap(zone.radius, zone.blendRadius, zone.blendCurve) * zone.weight;
                }
            }
            else
            {
                weights[i] = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        Target = null;
    }
}