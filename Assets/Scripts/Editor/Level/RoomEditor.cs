using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    private Room t;
    private Material gridMat;
    private LevelManager lvM;
    private PoolManager pM;
    private Board board;

    private float offsetSides = 100f;

    private float editorWidth { get { return EditorGUIUtility.currentViewWidth; } }

    private float minX { get { return offsetSides - 32; } }
    private float maxX { get { return editorWidth - offsetSides; } }

    private float minY = 220f;
    private float maxY = 770f;

    private Pool CurrentPool => PoolManager.Instance.GetLevelElementPoolAtIndex(t.SelectedBrush, t.SelectedTab);
    private LevelElement CurrentPoolElement => CurrentPool.prefab.GetComponent<LevelElement>();

    private int Columns()
    {
        return Board.Instance == null ? 15 : Board.Instance.Columns;
    }

    private int Rows()
    {
        return Board.Instance == null ? 15 : Board.Instance.Rows;
    }

    private void OnEnable()
    {
        t = target as Room;

        Shader shader = Shader.Find("Hidden/Internal-Colored");
        gridMat = new Material(shader);
    }

    private void OnDisable()
    {
        EditorUtility.SetDirty(t);
        DestroyImmediate(gridMat);
    }

    private float GridOffsetY()
    {
        return EditorGUIUtility.singleLineHeight * 10f;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        CheckRequiredStuff();
            t.FixLinks();
        //if(GUILayout.Button("Fix Links"))
        //{
        //    Undo.RecordObject(t, "Fix");
        //}
            serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(t);
            AssetDatabase.SaveAssets();
        }
    }

    private void CheckRequiredStuff()
    {
        lvM = LevelManager.Instance;
        pM = PoolManager.Instance;
        board = Board.Instance;

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
                    if (!board)
                    {
                        EditorGUILayout.HelpBox("No Board found. Make sure one is present in the scene.", MessageType.Error);

                    }
                    else
                    {
                        UpdateListElements();
                        DrawInspector();
                    }
                }
            }
        }

    }

    private void UpdateListElements()
    {
        int count = Board.Instance.Columns * Board.Instance.Rows;
        UpdateList(t.tileElements, count);
        UpdateList(t.entityElements, count);
    }

    private void UpdateList(List<LevelElementRoomSettings> list, int count)
    {
        if (list.Count != count)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                list.Add(new LevelElementRoomSettings());
            }
        }
    }

    private void DrawInspector()
    {
        DrawTitle();
        DrawGeneralSettings();
        DrawTabSettings();
        DrawBrushSettings();
        DrawClearAllButton();
        FillAllButton();
        float height = 700f;
        Rect rect = GUILayoutUtility.GetRect(800f, 1000, height, height);
        DrawButtons();
    }

    private void DrawTitle()
    {
        CustomEditorUtility.DrawTitle(t.name);
    }

    private void DrawGeneralSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.EndVertical();
    }

    private void DrawTabSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Current Element type:", EditorStyles.boldLabel);

        List<string> possibleTabs = new List<string>();
        possibleTabs.Add("Tiles");
        possibleTabs.Add("Entities");

        t.SelectedTab = GUILayout.Toolbar(t.SelectedTab, possibleTabs.ToArray());

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
                GameObject prefab = pM.pools[i].prefab;
                if (prefab)
                {
                    if (prefab.GetComponentInChildren<LevelElement>())
                    {
                        if (t.SelectedTab == 0)
                        {
                            if (prefab.GetComponentInChildren<Tile>())
                            {
                                brushes.Add(pM.pools[i].prefab.name);
                                colors.Add(pM.pools[i].prefab.GetComponentInChildren<LevelElement>().ColorInEditor());
                            }
                        }
                        else
                        {
                            if (!prefab.GetComponentInChildren<Tile>())
                            {
                                brushes.Add(pM.pools[i].prefab.name);
                                colors.Add(pM.pools[i].prefab.GetComponentInChildren<LevelElement>().ColorInEditor());
                            }
                        }
                    }
                }
            }
        }
        string[] brushesArray = brushes.ToArray();

        Color colorBrush = defaultCol;
        if (colors.Count > t.SelectedBrush)
        {
            colorBrush = colors[t.SelectedBrush];
        }

        GUI.color = colorBrush;
        EditorGUILayout.BeginVertical("box");
        GUI.color = defaultCol;

        EditorGUILayout.LabelField("Current Brush: ", EditorStyles.boldLabel);
        GUI.color = colorBrush;
        //c.selectedBrush = EditorGUILayout.Popup(c.selectedBrush, brushesArray);
        t.SelectedBrush = GUILayout.Toolbar(t.SelectedBrush, brushesArray, GUILayout.MinHeight(30));
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
        Rect rect = new Rect(new Vector2(offsetSides / 2, maxY + 40), new Vector2(editorWidth - offsetSides, EditorGUIUtility.singleLineHeight));
        if (GUI.Button(rect, "Clear All"))
        {
            Undo.RecordObject(t, "Clear All");

            if (t.SelectedTab == 0)
                t.tileElements.Clear();
            else
                t.entityElements.Clear();
        }
    }

    private void FillAllButton()
    {
        Rect rect = new Rect(new Vector2(offsetSides / 2, maxY + 60), new Vector2(editorWidth - offsetSides, EditorGUIUtility.singleLineHeight));
        if (GUI.Button(rect, "Fill All"))
        {
            Undo.RecordObject(t, "Fill All");
            FillList(t.CurrentList);
        }
    }

    private void FillList(List<LevelElementRoomSettings> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].levelElement = CurrentPoolElement;
        }
    }

    private void DrawButtons()
    {
        float gridWidth = maxX - minX;
        float gridHeight = maxY - minY;

        float width = gridWidth / Columns();
        float height = gridHeight / Rows();
        for (int x = 0; x < Columns(); x++)
        {
            for (int y = 0; y < Rows(); y++)
            {
                if (t.SelectedTab == 0)
                    DrawLevelElementButton(width, height, y, x, t.tileElements);
                else
                    DrawLevelElementButton(width, height, y, x, t.entityElements);
            }
        }
    }

    private void DrawLevelElementButton(float width, float height, int y, int x, List<LevelElementRoomSettings> listElement)
    {
        Color defaultCol = GUI.color;
        float xPos = Utility.Interpolate(minX, maxX, 0, Columns() - 1, x);
        float yPos = Utility.Interpolate(maxY, minY, 0, Rows() - 1, y);
        Vector2 pos = new Vector2(xPos - 10, yPos - height / 2);
        Vector2 size = new Vector2(width, height);
        Rect newRect = new Rect(pos, size);
        Rect buttonStatsRect = new Rect(pos - Vector2.up * 10, size);

        int indexElement = y + Rows() * x;
        Pool pool = CurrentPool;

        if (pool != null)
        {
            if (indexElement < listElement.Count)
            {
                if (listElement[indexElement] != null)
                {
                    LevelElement current = listElement[indexElement].levelElement;
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
                    }


                    EditorGUI.DrawRect(newRect, colorButton);

                    int order = listElement[indexElement].orderInTurn;

                    GUIStyle style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = Color.black;
                    style.fontStyle = FontStyle.Bold;
                    if(order != 0) EditorGUI.LabelField(newRect, order.ToString(), style);

                    if (newRect.Contains(currentEvent.mousePosition))
                    {
                        if (currentEvent.button == 1 && currentEvent.isMouse)
                        {
                            Undo.RecordObject(t, "Clear Level Element");

                            listElement[indexElement] = null;
                        }

                        if (currentEvent.button == 0 && currentEvent.isMouse)
                        {

                            if (currentEvent.shift)
                            {
                                if (listElement[indexElement].levelElement != null)
                                {
                                    SerializedProperty elements = serializedObject.FindProperty("entityElements");
                                    SerializedProperty element = elements.GetArrayElementAtIndex(indexElement);
                                    LevelElementWindow.Init(element, t);
                                }
                            }

                            else
                            {

                                Undo.RecordObject(t, "Add Level Element");

                                GameObject prefab = pool.prefab;

                                LevelElement lvl = prefab.GetComponentInChildren<LevelElement>();

                                if (lvl)
                                {
                                    listElement[indexElement].levelElement = lvl;
                                    listElement[indexElement].name = prefab.name;
                                }
                            }
                        }

                        if (currentEvent.button == 2 && currentEvent.isMouse)
                        {
                            Undo.RecordObject(t, "Change Brush");
                            if (listElement[indexElement].levelElement != null)
                            {
                                t.SelectedBrush = PoolManager.Instance.GetIndexLevelElementPool(listElement[indexElement].levelElement);
                            }
                        }
                    }

                    if (indexElement < listElement.Count)
                        if (listElement[indexElement] != null)
                        {
                            listElement[indexElement].pos = new Vector3(x, y, 0);
                        }
                }
            }
        }

        GUI.color = defaultCol;
    }
}
