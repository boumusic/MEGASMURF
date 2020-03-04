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
        if (property.isExpanded)
        {
            return 540f;
        }

        else
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Range range = fieldInfo.GetValue(property.serializedObject.targetObject) as Range;
        float viewWidth = EditorGUIUtility.currentViewWidth;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        int size = property.FindPropertyRelative("size").intValue;
        property.isExpanded = EditorGUI.Foldout(new Rect(position.position, new Vector2(viewWidth, lineHeight)), property.isExpanded, "Range", EditorStyles.foldoutHeader);

        if(property.isExpanded)
        {
            Rect sRect = new Rect(position.position + CustomEditorUtility.LineOffsetVertical(1), CustomEditorUtility.RectSize());
            EditorGUI.PropertyField(sRect, property.FindPropertyRelative("size"));

            DrawButtons(position, property, range, viewWidth, lineHeight, size);
            Color def = GUI.color;
            GUI.color = CustomEditorUtility.RemoveButtonColor();
            Rect clearAllRect = new Rect(new Vector2(position.x, position.y + 500f), new Vector2(EditorGUIUtility.currentViewWidth - 32, EditorGUIUtility.singleLineHeight));
            if (GUI.Button(clearAllRect, "Clear all"))
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Clear Range");
                range.coords.Clear();
            }
            GUI.color = def;
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
                    
                    Color buttonColor = new Color(0f, 0f, 0f, 0.4f);

                    for (int i = 0; i < range.coords.Count; i++)
                    {
                        if (range.coords[i] == new Vector2(x, y))
                        {

                            buttonColor = new Color(1f, 0.5f, 0.6f, 1f);
                            break;
                        }
                    }

                    GUI.color = buttonColor;
                    Rect button = new Rect(buttonPos, buttonScale);

                    Vector2 coord = new Vector2(x, y);


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

                        float added = 0.2f;
                        buttonColor += new Color(added, added, added, 0f);
                    }
                    EditorGUI.DrawRect(button, buttonColor);

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
