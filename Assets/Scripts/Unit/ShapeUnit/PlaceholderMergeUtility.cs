using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMergeUtility : MonoBehaviour
{
    public ShapeUnit selected;
    public ShapeUnit target;
    public ShapeUnit secondUnit;

    private void Start()
    {
        Stack<Tile> path = new Stack<Tile>();
        for (int i = 1; i < 6; i++)
        { 
            path.Push(Board.Instance.GetTile(i, 0));
        }

        selected.MoveTo(path, PathComplete);

        //Invoke("TakeDamage", 1);
    }

    private void TakeDamage()
    {
        selected.TakeDamage(target);
    }

    private void PathComplete()
    {
        target.InitiateMergeAlly(selected, MergeComplete);
    }

    private void MergeComplete()
    {
        Stack<Tile> path = new Stack<Tile>();
        for (int i = 10; i > 0; i--)
        {
            path.Push(Board.Instance.GetTile(i, 0));
        }
        target.MoveTo(path, SecondPathComplete);
        //StartCoroutine(MovingTo(path));
    }

    private IEnumerator MovingTo(Stack<Tile> path)
    {
        yield return new WaitForSeconds(1f);

    }


    private void SecondPathComplete()
    {
        target.InitiateMergeAlly(secondUnit);
    }
}
