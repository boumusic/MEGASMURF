using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMergeUtility : MonoBehaviour
{
    public ShapeUnit selected;
    public ShapeUnit target;

    private void Start()
    {
        Stack<Tile> path = new Stack<Tile>();
        for (int i = 1; i < 6; i++)
        { 
            path.Push(Board.Instance.GetTile(i, 0));
        }

        selected.MoveTo(path, PathComplete);
    }

    private void PathComplete()
    {
        target.InitiateMergeAlly(selected, MergeComplete);
    }

    private void MergeComplete()
    {
        Stack<Tile> path = new Stack<Tile>();
        for (int i = 1; i < 6; i++)
        {
            path.Push(Board.Instance.GetTile(i, 0));
        }
        //target.MoveTo(path);
    }
}
