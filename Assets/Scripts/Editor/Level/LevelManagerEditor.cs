using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private LevelManager m;

    private void OnEnable()
    {
        m = target as LevelManager;
    }

    private void OnSceneGUI()
    {
        DrawHandles();
    }

    private void DrawHandles()
    {
        Color defaultCol = Handles.color;
        Color handleCol = new Color(1, 1, 1, 0.4f);
        if (m.settings)
        {
            float maxPosY = m.settings.levelWidth;
            float kill = m.settings.killPos;
            float spawn = m.settings.spawnPos;
            DrawEdge(maxPosY, kill);
            DrawEdge(maxPosY, spawn);
            DrawRows(handleCol, kill, spawn, maxPosY);
            DrawColumns(handleCol, kill, spawn, maxPosY);
        }

        Handles.color = defaultCol;
    }

    private void DrawRows(Color handleCol, float kill, float spawn, float maxPosY)
    {
        for (int i = 0; i < LevelManager.chunkRows; i++)
        {
            float y = Utility.Interpolate(-maxPosY, maxPosY, 0, LevelManager.chunkRows - 1, i);
            Vector3 left = new Vector3(kill, y, 0);
            Vector3 right = new Vector3(spawn, y, 0);

            Handles.color = handleCol;
            Handles.DrawLine(left, right);

            Vector3 posSphereSpawn = new Vector3(m.settings.spawnPos, y, 0f);
            Vector3 posSphereKill = new Vector3(m.settings.killPos, y, 0f);

            Handles.color = LevelManager.spawnCol;
            Handles.SphereHandleCap(1, posSphereSpawn, Quaternion.identity, 0.35f, EventType.Repaint);

            Handles.ArrowHandleCap(1, posSphereSpawn, Quaternion.LookRotation(Vector3.left, Vector3.up), 1.5f, EventType.Repaint);

            Handles.color = LevelManager.killCol;
            Handles.SphereHandleCap(1, posSphereKill, Quaternion.identity, 0.35f, EventType.Repaint);

            Handles.color = handleCol;
        }
    }

    private void DrawColumns(Color handleCol, float kill, float spawn, float maxPosY)
    {
        Handles.color = handleCol;
        int columnKill = (int)(Mathf.Abs(kill) / m.settings.columnSpacing);
        int columnSpawn = (int)(Mathf.Abs(spawn) / m.settings.columnSpacing);

        for (int i = 0; i <= columnKill; i++)
        {
            DrawColumn(maxPosY, i, -1);
        }

        for (int i = 1; i <= columnSpawn; i++)
        {
            DrawColumn(maxPosY, i, 1);
        }
    }

    private void DrawColumn(float maxPosY, int i, int mul)
    {
        float horiz = (i * m.settings.columnSpacing) * mul;
        Vector3 top = new Vector3(horiz, maxPosY, 0);
        Vector3 bottom = new Vector3(horiz, -maxPosY, 0);
        Handles.DrawLine(top, bottom);
    }

    private void DrawEdge(float maxPosY, float x)
    {
        if (x < 0) Handles.color = LevelManager.killCol;
        else Handles.color = LevelManager.spawnCol;

        Vector3 killTop = new Vector3(x, maxPosY, 0f);
        Vector3 killBottom = new Vector3(x, -maxPosY, 0f);
        Handles.DrawLine(killTop, killBottom);
    }
}
