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

        Color def = GUI.color;
        if(t.search != "")
        {
            GUI.color = Color.gray;
        }

        EditorGUILayout.BeginVertical("box");

        GUI.color = def;

        for (int i = 0; i < names.arraySize; i++)
        {
            bool canDraw = true;
            if(t.search != "")
            {
                canDraw = false;
                if(t.names[i].Contains(t.search))
                {
                    canDraw = true;
                }
            }

            if(canDraw)
            {
                SerializedProperty name = names.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(name);
                if (CustomEditorUtility.RemoveButton())
                {
                    Undo.RecordObject(t, "Remove Name");
                }
                EditorGUILayout.EndHorizontal();
            }            
        }

        EditorGUILayout.EndVertical();

        if(CustomEditorUtility.AddButton())
        {
            Undo.RecordObject(t, "Add Button");
            t.names.Add("");
        }

		serializedObject.ApplyModifiedProperties();
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(t);
		}
	}
}
