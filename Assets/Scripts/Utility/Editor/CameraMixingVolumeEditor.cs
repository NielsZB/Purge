using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraMixingVolume))]
public class CameraMixingVolumeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        var mixingVolume = target as CameraMixingVolume;

        if (mixingVolume.zones.Length > 0)
        {
            for (int i = 0; i < mixingVolume.zones.Length; i++)
            {

                CameraMixingVolume.Zone zone = mixingVolume.zones[i];



                EditorGUI.BeginChangeCheck();
                Vector3 position = Handles.PositionHandle(zone.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(mixingVolume, "Change position");
                    zone.position = position;
                }
                EditorGUI.BeginChangeCheck();
                Handles.color = Color.grey;
                float radius = Handles.RadiusHandle(Quaternion.identity, zone.position, zone.radius);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(mixingVolume, "Change radius");
                    zone.radius = radius;
                }
                float outerRadius = zone.radius + zone.mixWidth;
                EditorGUI.BeginChangeCheck();
                Handles.color = Color.white;
                float mixRadius = Handles.RadiusHandle(Quaternion.identity, zone.position, outerRadius);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(mixingVolume, "Change mixRadius");
                    zone.mixWidth = mixRadius;
                }

                mixingVolume.zones[i] = zone;

                GUIStyle centeredStyle = GUIStyle.none;
                centeredStyle.alignment = TextAnchor.LowerCenter;

                if (zone.camera != null)
                {
                    Handles.Label(zone.position, "Zone " + zone.camera.name, centeredStyle);
                }
                else
                {

                    Handles.Label(zone.position, "Zone " + i.ToString() + " camera needed!", centeredStyle);
                }
            }

        }
    }
}
