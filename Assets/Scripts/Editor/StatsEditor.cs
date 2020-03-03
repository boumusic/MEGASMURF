using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stats))]
public class StatsEditor : Editor
{
	private Stats t;

	private void OnEnable()
	{
		t = target as Stats;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		CustomEditorUtility.DrawTitle(t.name);
		base.OnInspectorGUI();
		serializedObject.ApplyModifiedProperties();
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(t);
		}
	}
}
