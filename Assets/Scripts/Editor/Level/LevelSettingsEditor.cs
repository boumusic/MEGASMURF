using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(LevelSettings))]
public class LevelSettingsEditor : Editor
{
    private LevelSettings l;

    private void OnEnable()
    {
        l = target as LevelSettings;
    }

    public override void OnInspectorGUI()
    {
        DrawSettings();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSettings()
    {
        CustomEditorUtility.DrawTitle("Level Settings");

        GridSettings();

        EditorGUILayout.Space();
        SpawnAndDestroySettings();
        EditorGUILayout.Space();
        ScrollingSettings();
        
        CustomEditorUtility.QuickSerializeObject("levelChunks", serializedObject);
    }    

    private void ScrollingSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Difficulty", EditorStyles.boldLabel);
        CustomEditorUtility.CustomMinMaxSlider(ref l.minScrollingSpeed, ref l.maxScrollingSpeed, 0, 10, "Min & max scrolling speed", l);

        EditorGUILayout.Space();

        CustomEditorUtility.QuickSerializeObject("scrollingCurve", serializedObject);

        EditorGUILayout.Space();

        CustomEditorUtility.QuickSerializeObject("difficultyIncreaseSpeed", serializedObject);

        GUI.enabled = false;
        float seconds = 1f / l.difficultyIncreaseSpeed;
        int mins = (int)(seconds / 60);
        seconds -= mins * 60f;
        EditorGUILayout.LabelField("Maximum difficulty will be reached in " + mins.ToString() + " minutes and " + seconds.ToString("F1") + " seconds.");
        GUI.enabled = true;
        EditorGUILayout.EndVertical();
    }

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
}
