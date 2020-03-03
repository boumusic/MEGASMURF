using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    public ActionSequencer instance;

    private void Awake()
    {
        instance = this;
    }

    private Queue<Action<float>> ActionQueue;
}
