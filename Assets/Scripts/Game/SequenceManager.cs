using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    AutomaticResume,
    ManualResume
}

public class SequenceManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance { get { if (!instance) instance = FindObjectOfType<BattleManager>(); return instance; } }

    private Queue<Action> actionQueue;
    private Queue<ActionType> actionTypeQueue;

    public void EnQueueAction(Action action, ActionType actionType)
    {
        if(actionQueue.Count == 0 && actionTypeQueue.Count == 0)
        {
            if (actionTypeQueue.Dequeue() == ActionType.AutomaticResume)
                AutomaticAction(action);
            else
                ManualAction(action);
        }
        else
        {
            actionQueue.Enqueue(action);
            actionTypeQueue.Enqueue(actionType);
        }
        
    }

    private void AutomaticAction(Action action)
    {
        action?.Invoke();
        Resume();
    }

    private void ManualAction(Action action)
    {
        action?.Invoke();
    }

    public void Resume()
    {
        if(actionQueue.Count > 0 && actionTypeQueue.Count > 0)
        {
            if (actionTypeQueue.Dequeue() == ActionType.AutomaticResume)
                AutomaticAction(actionQueue.Dequeue());
            else
                ManualAction(actionQueue.Dequeue());
        }
    }

    public void Clear()
    {
        actionQueue.Clear();
        actionTypeQueue.Clear();
    }
}
