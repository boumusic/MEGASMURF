using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private Level l;

    private void OnEnable()
    {
        l = target as Level;
    }

    public override void OnInspectorGUI()
    {
        DrawSettings();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSettings()
    {
        CustomEditorUtility.DrawTitle("Level Settings");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        CustomEditorUtility.QuickSerializeObject("rooms", serializedObject);
    } 

    /*
    private void SpawnAndDestroySettings()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Spawn and destroy", EditorStyles.boldLabel);
        CustomEditorUtility.QuickSerializeObject("spawnPos", serializedObject);
        CustomEditorUtility.QuickSerializeObject("killPos", serializedObject);
        //CustomEditorUtility.QuickSerializeObject("delayBetweenEachElementSpawn", serializedObject);

        EditorGUILayout.EndVertical();
    }

    private void GridSettings()
    {
        
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
        CustomEditorUtility.QuickSerializeObject("columnSpacing", serializedObject);
        CustomEditorUtility.QuickSerializeObject("levelWidth", serializedObject);

        EditorGUILayout.EndVertical();
        
    }
    */
}
