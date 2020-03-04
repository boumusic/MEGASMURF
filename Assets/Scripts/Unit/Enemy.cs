using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Brain UnitBrain { get; set; }

    private void Awake()
    {
        UnitBrain = new Brain(this);
    }

    public override Color ColorInEditor()
    {
        return Color.red;
    }
}
