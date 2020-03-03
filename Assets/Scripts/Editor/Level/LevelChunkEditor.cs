using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LevelChunk))]
public class LevelChunkEditor : Editor
{
    private LevelChunk c;
    private Material gridMat;
    private LevelManager lvM;
    private PoolManager pM;

    private float offsetSides = 100f;

    private float editorWidth { get { return EditorGUIUtility.currentViewWidth; } }

    private float minX { get { return offsetSides - 32; } }
    private float maxX { get { return editorWidth - offsetSides; } }

    private float minY = 200f;
    private float maxY = 750f;

    private int columns = LevelManager.chunkColumns;
    private int rows = LevelManager.chunkRows;

    private void OnEnable()
    {
        c = target as LevelChunk;

        Shader shader = Shader.Find("Hidden/Internal-Colored");
        gridMat = new Material(shader);
    }

    private void OnDisable()
    {
        EditorUtility.SetDirty(c);
        DestroyImmediate(gridMat);
    }

    private float GridOffsetY()
    {
        return EditorGUIUtility.singleLineHeight * 10f;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        CheckRequiredStuff();
        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }

    private void CheckRequiredStuff()
    {
        lvM = LevelManager.Instance;
        pM = PoolManager.Instance;

        if (!pM)
        {
            EditorGUILayout.HelpBox("No Pool Manager found. Make sure one is present in the scene.", MessageType.Error);
        }
        else
        {
            if (!lvM)
            {
                EditorGUILayout.HelpBox("No Level Manager found. Make sure one is present in the scene.", MessageType.Error);
            }

            else
            {
                if (!lvM.settings)
                {
                    EditorGUILayout.HelpBox("Please assign a Level Settings object to the Level Manager.", MessageType.Error);
                }
                else
                {
                    UpdateListElements();
                    DrawInspector();
                }
            }
        }

    }

    private void UpdateListElements()
    {
        int count = LevelManager.chunkColumns * LevelManager.chunkRows;
        if (c.elements.Count != count)
        {
            c.elements.Clear();
            for (int i = 0; i < count; i++)
            {
                c.elements.Add(new LevelElementChunkSettings());
            }
        }
    }

    private void DrawInspector()
    {
        DrawTitle();
        DrawGeneralSettings();
        DrawBrushSettings();
        DrawClearAllButton();
        DrawGrid();
        DrawButtons();
    }

    private void DrawTitle()
    {
        CustomEditorUtility.DrawTitle("Level Chunk");
    }

    private void DrawGeneralSettings()
    {
        EditorGUILayout.BeginVertical("box");

        CustomEditorUtility.QuickSerializeObject("difficulty", serializedObject);

        EditorGUILayout.EndVertical();
    }

    private void DrawBrushSettings()
    {
        Color defaultCol = GUI.color;

        List<string> brushes = new List<string>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < pM.pools.Count; i++)
        {
            if (pM.pools[i].isLevelElement)
            {
                brushes.Add(pM.pools[i].prefab.name);
                if (pM.pools[i].prefab)
                {
                    if (pM.pools[i].prefab.GetComponentInChildren<LevelElement>())
                    {
                        colors.Add(pM.pools[i].prefab.GetComponentInChildren<LevelElement>().ColorInEditor());

                    }
                }
            }
        }
        string[] brushesArray = brushes.ToArray();

        Color colorBrush = defaultCol;
        if (colors.Count > c.selectedBrush)
        {
            colorBrush = colors[c.selectedBrush];
        }

        GUI.color = colorBrush;
        EditorGUILayout.BeginVertical("box");
        GUI.color = defaultCol;

        EditorGUILayout.LabelField("Current Brush: ", EditorStyles.boldLabel);
        GUI.color = colorBrush;
        //c.selectedBrush = EditorGUILayout.Popup(c.selectedBrush, brushesArray);
        c.selectedBrush = GUILayout.Toolbar(c.selectedBrush, brushesArray, GUILayout.MinHeight(30));
        GUIStyle centered = new GUIStyle();
        centered.alignment = TextAnchor.MiddleCenter;
        centered.fontSize = 10;

#if UNITY_PRO_LICENSE
        centered.normal.textColor = Color.white;
#endif

        EditorGUILayout.LabelField("Left click to paint, Shift+Left Click to add custom Perks, Right click to remove, Middle mouse to pick the brush.", centered);
        GUI.color = defaultCol;

        EditorGUILayout.EndVertical();

    }

    private void DrawClearAllButton()
    {

        Rect rect = new Rect(new Vector2(offsetSides / 2, maxY + 50), new Vector2(editorWidth - offsetSides, EditorGUIUtility.singleLineHeight * 2));
        if (GUI.Button(rect, "Clear All"))
        {
            Undo.RecordObject(c, "Clear All");
            c.elements.Clear();
        }
    }

    private void DrawButtons()
    {
        float size = (editorWidth - offsetSides * 2) / 10;
        float fixedHeight = 50;
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                DrawLevelElementButton(size, fixedHeight, y, x);
            }
        }
    }

    private void DrawLevelElementButton(float width, float height, int y, int x)
    {
        Color defaultCol = GUI.color;
        float xPos = Utility.Interpolate(minX, maxX, 0, columns - 1, x);
        float yPos = Utility.Interpolate(minY, maxY, 0, rows - 1, y);
        Vector2 pos = new Vector2(xPos - 10, yPos - height / 2);
        Vector2 size = new Vector2(width, height);
        Rect newRect = new Rect(pos, size);
        Rect buttonStatsRect = new Rect(pos - Vector2.up * 10, size / new Vector2(2, 3f));

        int indexElement = y + LevelManager.chunkRows * x;
        Pool pool = PoolManager.Instance.GetLevelElementPoolAtIndex(c.selectedBrush);

        if (pool != null)
        {
            if (indexElement < c.elements.Count)
            {
                if (c.elements[indexElement] != null)
                {
                    LevelElement current = c.elements[indexElement].levelElement;
                    Color colorButton = new Color();
                    if (current)
                        colorButton = current.ColorInEditor();
                    else
                    {
#if UNITY_PRO_LICENSE
                        colorButton = new Color(0f, 0f, 0f, 0.2f);
#else
                        colorButton = new Color(0f, 0f, 0f, 0.05f);
#endif
                    }


                    Event currentEvent = Event.current;
                    if (newRect.Contains(currentEvent.mousePosition))
                    {
                        colorButton *= 0.8f;

                        //if (GUI.Button(buttonStatsRect, "Stats")) { }
                    }


                    EditorGUI.DrawRect(newRect, colorButton);

                    if (newRect.Contains(currentEvent.mousePosition))
                    {
                        if (currentEvent.button == 1 && currentEvent.isMouse)
                        {
                            Undo.RecordObject(c, "Clear Level Element");

                            c.elements[indexElement] = null;
                        }

                        if (currentEvent.button == 0 && currentEvent.isMouse)
                        {
                            if (currentEvent.shift)
                            {
                                if (c.elements[indexElement].levelElement != null)
                                {
                                    SerializedProperty elements = serializedObject.FindProperty("elements");
                                    SerializedProperty element = elements.GetArrayElementAtIndex(indexElement);
                                    LevelElementWindow.Init(element, c);

                                }
                            }

                            else
                            {
                                Undo.RecordObject(c, "Add Level Element");

                                GameObject prefab = pool.prefab;

                                LevelElement lvl = prefab.GetComponentInChildren<LevelElement>();

                                if (lvl)
                                {
                                    c.elements[indexElement].levelElement = lvl;
                                }
                            }
                        }

                        if (currentEvent.button == 2 && currentEvent.isMouse)
                        {
                            Undo.RecordObject(c, "Change Brush");
                            if (c.elements[indexElement].levelElement != null)
                            {
                                c.selectedBrush = PoolManager.Instance.GetIndexLevelElementPool(c.elements[indexElement].levelElement);
                            }
                        }
                    }

                    if (indexElement < c.elements.Count)
                        if (c.elements[indexElement] != null)
                        {
                            c.elements[indexElement].pos = new Vector3(x, y, 0);
                        }
                }
            }
        }

        GUI.color = defaultCol;
    }

    private void DrawGrid()
    {
        float height = 1000f;
        Rect rect = GUILayoutUtility.GetRect(800f, 1000, height, height);
        float offsetY = GridOffsetY();
        //Rect rect = new Rect(0, 0, editorWidth, 900f);
        //Rect rect = new Rect(new Vector2(minX, minY), new Vector2(maxX - minX, maxY - minY));
        if (Event.current.type == EventType.Repaint)
        {
            if (gridMat)
            {
                GUI.BeginClip(rect);
                GL.PushMatrix();
                GL.Clear(true, false, Color.red);
                gridMat.SetPass(0);


                GL.Begin(GL.LINES);
                for (int x = 0; x < columns; x++)
                {
                    float xPos = Utility.Interpolate(minX, maxX, 0, columns - 1, x);
                    Vector3 start = new Vector3(xPos, minY - offsetY, 0f);
                    Vector3 end = new Vector3(xPos, maxY - offsetY, 0f);
                    Color col = LevelManager.gridCol;
                    if (x == columns - 1) col = LevelManager.spawnCol;
                    else if (x == 0) col = LevelManager.killCol;

                    DrawLine(start, end, col);
                }

                for (int y = 0; y < rows; y++)
                {
                    float yPos = Utility.Interpolate(minY, maxY, 0, rows - 1, y) - offsetY;
                    Vector3 start = new Vector3(minX, yPos, 0f);
                    Vector3 end = new Vector3(maxX, yPos, 0f);

                    DrawLine(start, end, LevelManager.gridCol);
                }
                GL.End();

                //ARROWS
                float tSize = 10;
                float tOffset = 30;
                for (int y = 0; y < rows; y++)
                {
                    float yPos = Utility.Interpolate(minY, maxY, 0, rows - 1, y) - offsetY;
                    Vector3 a = new Vector3(maxX - tOffset, yPos + tSize / 2, 0f);
                    Vector3 b = new Vector3(maxX - tSize - tOffset, yPos, 0f);
                    Vector3 c = new Vector3(maxX - tOffset, yPos - tSize / 2, 0f);
                    DrawTriangle(a, b, c, LevelManager.spawnCol);
                }

                GL.PopMatrix();
                GUI.EndClip();
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color col)
    {
        GL.Color(col);
        GL.Vertex(start);
        GL.Vertex(end);
    }

    private void DrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        GL.Begin(GL.TRIANGLES);
        GL.Color(color);

        GL.Vertex(a);
        GL.Vertex(b);
        GL.Vertex(c);

        GL.End();
    }

    private void DrawSquare(Vector3 pos, float size, Color color)
    {
        GL.Begin(GL.QUADS);
        GL.Color(color);

        Vector3 bottomLeft = new Vector3(pos.x - size, pos.y - size, 0f);
        Vector3 bottomRight = new Vector3(pos.x + size, pos.y - size, 0f);
        Vector3 topLeft = new Vector3(pos.x - size, pos.y + size, 0f);
        Vector3 topRight = new Vector3(pos.x + size, pos.y + size, 0f);

        GL.Vertex(bottomLeft);
        GL.Vertex(bottomRight);
        GL.Vertex(topRight);
        GL.Vertex(topLeft);

        GL.End();
    }

}
