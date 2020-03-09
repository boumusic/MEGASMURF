using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitNameSettings))]
public class UnitNameSettingsEditor : Editor
{
    private UnitNameSettings t;

    private void OnEnable()
    {
        t = target as UnitNameSettings;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        CustomEditorUtility.DrawTitle("Unit names");


        CustomEditorUtility.QuickSerializeObject("search", serializedObject);

        EditorGUILayout.Space();

        SerializedProperty names = serializedObject.FindProperty("names");
        SerializedProperty prefixes = serializedObject.FindProperty("prefixes");

        Color def = GUI.color;
        if (t.search != "")
        {
            GUI.color = Color.gray;
        }


        GUI.color = def;

        DrawNameArray(names, t.names, "Names");
        DrawNameArray(prefixes, t.prefixes, "Prefixes");

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Unit's name coming first (Jean-Pierre or Pierre Jean) (Level 2 Unit).");
        CustomEditorUtility.QuickSerializeObject("nameComingFirst", serializedObject);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Name to use after the prefix (Level 3 unit).");
        CustomEditorUtility.QuickSerializeObject("nameForPrefix", serializedObject);

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(t);
        }
    }

    private void DrawNameArray(SerializedProperty names, List<string> namesList, string title)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        for (int i = 0; i < names.arraySize; i++)
        {
            bool canDraw = true;
            if (t.search != "")
            {
                canDraw = false;
                if (namesList[i].Contains(t.search))
                {
                    canDraw = true;
                }
            }

            if (canDraw)
            {
                SerializedProperty name = names.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(name);
                if (CustomEditorUtility.RemoveButton())
                {
                    Undo.RecordObject(t, "Remove Name");
                    namesList.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndVertical();

        if (CustomEditorUtility.AddButton())
        {
            Undo.RecordObject(t, "Add Button");
            namesList.Add("");
        }
    }
}
