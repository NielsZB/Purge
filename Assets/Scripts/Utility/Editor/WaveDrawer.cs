using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Wave))]
public class WaveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty count = property.FindPropertyRelative("count");
        SerializedProperty duration = property.FindPropertyRelative("duration");

        float labelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = labelWidth - 60;
        Rect costRect = new Rect(position.x, position.y, (position.width / 3) + 30, EditorGUIUtility.singleLineHeight);
        Rect durationRect = new Rect((position.width / 3) * 2 + 5, position.y, position.width / 3 + 30, EditorGUIUtility.singleLineHeight);


        EditorGUI.PropertyField(costRect, count);
        EditorGUI.PropertyField(durationRect, duration);

        EditorGUI.EndProperty();
    }
}
