using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelElementWindow : EditorWindow
{
    private SerializedProperty element;
    private Object obj;
    public static void Init(SerializedProperty elem, Object obj)
    {
        LevelElementWindow window = (LevelElementWindow)EditorWindow.GetWindow(typeof(LevelElementWindow));
        window.element = elem;
        window.obj = obj;
        window.Show();
    }

    private void OnGUI()
    {
        CustomEditorUtility.DrawTitle("Level Element Perks");

        SerializedProperty stats = element.FindPropertyRelative("stats");
        for (int i = 0; i < stats.arraySize; i++)
        {
            float curr = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 2f;
            EditorGUILayout.BeginHorizontal();
            SerializedProperty stat = stats.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(stat);
            if (GUILayout.Button("X", GUILayout.MaxWidth(40)))
            {
                stats.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = curr;
        }

        EditorGUILayout.BeginHorizontal();

        if (stats.arraySize > 0)
        {
            if (GUILayout.Button("Clear"))
            {
                Undo.RecordObject(obj, "Clear");
                stats.ClearArray();
            }
        }

        if (GUILayout.Button("Add"))
        {
            Undo.RecordObject(obj, "Add Element");
            stats.InsertArrayElementAtIndex(stats.arraySize);
        }


        EditorGUILayout.EndHorizontal();
        element.serializedObject.ApplyModifiedProperties();
    }
}
