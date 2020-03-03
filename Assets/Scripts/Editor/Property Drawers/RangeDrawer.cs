using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Range))]
public class RangeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return CustomEditorUtility.PropertyHeight(3 + Length(property));
        return 800f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Range range = fieldInfo.GetValue(property.serializedObject.targetObject) as Range;
        float viewWidth = EditorGUIUtility.currentViewWidth;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        //float buttonOffset = 5;
        int size = property.FindPropertyRelative("size").intValue;

        EditorGUI.LabelField(new Rect(position.position, new Vector2(viewWidth, lineHeight)), "Range", EditorStyles.boldLabel);
        Rect sRect = new Rect(position.position + CustomEditorUtility.LineOffsetVertical(1), CustomEditorUtility.RectSize());
        EditorGUI.PropertyField(sRect, property.FindPropertyRelative("size"));

        DrawButtons(position, property, range, viewWidth, lineHeight, size);
        
        if(GUI.Button(new Rect(0, 500, 50, 50),"Clear all"))
        {
            Undo.RecordObject(property.serializedObject.targetObject, "Clear Range");
            range.coords.Clear();
        }
    }

    private void DrawButtons(Rect position, SerializedProperty property, Range range, float viewWidth, float lineHeight, int size)
    {
        int length = Length(property);
        for (int x = -size; x < size + 1; x++)
        {
            for (int y = -size; y < size + 1; y++)
            {
                bool middle = x == 0 && y == 0;
                if (!middle)
                {
                    float ratio = 0.2f;

                    float buttonSize = ((viewWidth * (1 - ratio)) / length) * 0.7f;
                    float minX = viewWidth * ratio;
                    float maxX = viewWidth - viewWidth * ratio - buttonSize;

                    float xPos = Utility.Interpolate(minX, maxX, -size, size, x);
                    float gridSize = maxX - minX;

                    float yOffset = position.y + lineHeight * 3;
                    float yPos = Utility.Interpolate(yOffset, yOffset + gridSize, -size, size, y);
                    Vector2 buttonPos = new Vector2(xPos, yPos);
                    Vector2 buttonScale = new Vector2(buttonSize, buttonSize);

                    Color def = GUI.color;
                    float value = 0.55f;
                    
                    Color buttonColor = new Color(value, value, value, 1f);

                    for (int i = 0; i < range.coords.Count; i++)
                    {
                        if (range.coords[i] == new Vector2(x, y))
                        {
                            buttonColor = new Color(1f, 0.23f, 0.35f, 1f);
                            break;
                        }
                    }

                    GUI.color = buttonColor;
                    Rect button = new Rect(buttonPos, buttonScale);

                    Vector2 coord = new Vector2(x, y);

                    EditorGUI.DrawRect(button, buttonColor);

                    Event e = Event.current;

                    if(button.Contains(e.mousePosition))
                    {
                        if(e.isMouse)
                        {
                            if (e.button == 0)
                            {
                                Undo.RecordObject(property.serializedObject.targetObject, "Add Range");
                                if (!range.coords.Contains(coord))
                                    range.coords.Add(coord);
                            }

                            else if (e.button == 1)
                            {
                                Undo.RecordObject(property.serializedObject.targetObject, "Remove Range");
                                range.coords.Remove(coord);
                            }
                        }                        
                    }

                    GUI.color = def;
                }
            }
        }
    }

    private int Length(SerializedProperty property)
    {
        int length = property.FindPropertyRelative("size").intValue * 2 + 1;
        return length;
    }
}
