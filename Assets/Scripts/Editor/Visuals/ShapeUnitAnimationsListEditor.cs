using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeUnitAnimationsList))]
public class ShapeUnitAnimationsListEditor : Editor
{
    private ShapeUnitAnimationsList t;

    private void OnEnable()
    {
        t = target as ShapeUnitAnimationsList;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        CustomEditorUtility.DrawTitle("Shape Animations");

        SerializedProperty animations = serializedObject.FindProperty("animations");

        for (int i = 0; i < animations.arraySize; i++)
        {
            SerializedProperty anim = animations.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            anim.isExpanded = EditorGUILayout.Foldout(anim.isExpanded, t.animations[i].name, EditorStyles.foldoutHeader);
            if(CustomEditorUtility.RemoveButton())
            {
                Undo.RecordObject(t, "Remove");
                t.animations.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
            if (anim.isExpanded)
            {
                CustomEditorUtility.QuickSerializeRelative("name", anim);
                CustomEditorUtility.QuickSerializeRelative("legs", anim);
                CustomEditorUtility.QuickSerializeRelative("arms", anim);
            }
            EditorGUILayout.EndVertical();
        }

        if(CustomEditorUtility.AddButton())
        {
            Undo.RecordObject(t, "Add");
            t.animations.Add(new ShapeUnitAnimation());
        }

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(t);
        }
    }
}
