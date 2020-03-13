using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombito : Enemy
{
    public override void OnEnable()
    {
        base.OnEnable();
        UnitAnimator.gameObject.SetActive(true);
    }
}
