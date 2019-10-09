using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpatialBlend))]
public class SpatialBlendEditor : Editor
{
    public void OnSceneGUI()
    {
        var spatialBlend = target as SpatialBlend;
        var transform = spatialBlend.transform;
        for (int i = 0; i < spatialBlend.zones.Length; i++)
        {
            Undo.RecordObject(spatialBlend, "Spatial Zone Modification");
            SpatialBlend.Zone zone = spatialBlend.zones[i];
            Handles.Label(spatialBlend.zones[i].point, spatialBlend.zones[i].name);
            zone.point = Handles.PositionHandle(zone.point, Quaternion.identity);
            Handles.color = Color.white;
            zone.radius = Handles.RadiusHandle(Quaternion.identity, zone.point, zone.radius);
            Handles.color = Color.grey;
            zone.blendRadius = Handles.RadiusHandle(Quaternion.identity, zone.point, zone.blendRadius);
            if (zone.blendRadius > zone.radius)
            {
                zone.blendRadius = zone.radius;
            }

            spatialBlend.zones[i] = zone;
        }
    }
}
