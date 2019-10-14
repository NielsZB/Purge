using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class BlendVolume : MonoBehaviour
{
    public enum VolumeType
    {
        directionIndepentant,
        singleDirectional,
        multiDirectional
    }
    [SerializeField] VolumeType type;
    [SerializeField] float blendDistance = 1f;
    [SerializeField] Vector3 sizeRatio = Vector3.one;

    [ShowNativeProperty]
    public float BlendValue { get; private set; }

    const float rayDistance = 10000f;

    MeshCollider innerMeshCollider;
    MeshCollider outerMeshCollider;

    Mesh outerMesh;
    void Awake()
    {
        GetComponent<Renderer>().enabled = false;
        innerMeshCollider = GetComponent<MeshCollider>();

        if (innerMeshCollider != null)
        {
            GameObject outerObject = new GameObject(name + "_OuterCollider");
            outerObject.transform.SetParent(transform, false);
            outerObject.layer = gameObject.layer;

            outerMeshCollider = outerObject.AddComponent<MeshCollider>();

            if (outerMesh == null)
            {
                CreateOuterMesh();
            }

            outerMeshCollider.sharedMesh = outerMesh;
            outerMeshCollider.convex = true;
            outerMeshCollider.isTrigger = true;

            innerMeshCollider.convex = true;
            innerMeshCollider.isTrigger = true;
        }
    }


    void CreateOuterMesh()
    {
        if (outerMesh == null)
        {
            outerMesh = new Mesh
            {
                name = innerMeshCollider.sharedMesh.name + " Clone"
            };
        }
        Vector3[] vertices = new Vector3[innerMeshCollider.sharedMesh.vertices.Length];
        Vector3[] normals = new Vector3[innerMeshCollider.sharedMesh.normals.Length];
        int[] triangles = new int[innerMeshCollider.sharedMesh.triangles.Length];

        Mesh innerMesh = innerMeshCollider.sharedMesh;

        // create scaled vertices
        for (int i = 0; i < innerMesh.vertices.Length; i++)
        {
            vertices[i] = innerMesh.vertices[i] + (innerMesh.normals[i] * blendDistance);
            vertices[i].x *= sizeRatio.x;
            vertices[i].y *= sizeRatio.y;
            vertices[i].z *= sizeRatio.z;
        }

        // create normals
        for (int i = 0; i < innerMesh.normals.Length; i++)
        {
            normals[i] = innerMesh.normals[i];
        }

        // create triangles
        for (int i = 0; i < innerMesh.triangles.Length; i++)
        {
            triangles[i] = innerMesh.triangles[i];
        }


        outerMesh.vertices = vertices;
        outerMesh.normals = normals;
        outerMesh.triangles = triangles;
        outerMesh.RecalculateBounds();
        outerMesh.RecalculateNormals();
        outerMesh.RecalculateTangents();
    }
    public float GetBlendValue(Vector3 point, VolumeType type = VolumeType.directionIndepentant, bool useTrigger = false)
    {
        if (useTrigger)
        {
            if (!outerMeshCollider.bounds.Contains(point))
            {
                return 0f;
            }
        }
        else
        {
            if (!outerMeshCollider.bounds.Contains(point))
            {
                return 0f;
            }
        }

        Vector3 closesPoint = innerMeshCollider.ClosestPoint(point);

        if (closesPoint == point)
        {
            return 1f;
        }

        Vector3 rayDirection = (closesPoint - point).normalized;

        Ray ray = new Ray
        {
            origin = point - rayDirection * rayDistance,
            direction = rayDirection * rayDistance
        };

        outerMeshCollider.Raycast(ray, out RaycastHit hit, rayDistance);

        float distanceToInner = (hit.point - closesPoint).magnitude;
        float distanceToPoint = (hit.point - point).magnitude;
        return Mathf.Clamp01(distanceToPoint / distanceToInner);
    }

    public void ApplyBlendDistance(float newBlendDistance)
    {
        blendDistance = newBlendDistance;
        Mesh innerMesh = innerMeshCollider.sharedMesh;

        Vector3[] vertices = new Vector3[innerMeshCollider.sharedMesh.vertices.Length];
        for (int i = 0; i < outerMesh.vertices.Length; i++)
        {
            vertices[i] = innerMesh.vertices[i] + (innerMesh.normals[i] * blendDistance);
            vertices[i].x *= sizeRatio.x;
            vertices[i].y *= sizeRatio.y;
            vertices[i].z *= sizeRatio.z;
        }

        outerMesh.vertices = vertices;

        outerMesh.RecalculateBounds();
        outerMesh.RecalculateNormals();
        outerMesh.RecalculateTangents();
    }
}
