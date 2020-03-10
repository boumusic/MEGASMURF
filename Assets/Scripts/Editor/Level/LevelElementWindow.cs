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
        CustomEditorUtility.DrawTitle("Level Element Properties");

        SerializedProperty order = element.FindPropertyRelative("orderInTurn");
        EditorGUILayout.PropertyField(order);
        
        element.serializedObject.ApplyModifiedProperties();
    }
}
