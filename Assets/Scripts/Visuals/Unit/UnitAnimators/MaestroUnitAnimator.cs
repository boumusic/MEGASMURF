using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaestroUnitAnimator : UnitAnimator
{
    [Header("Components")]
    public Animator animator;

    public void SpellAnim()
    {
        animator.SetTrigger("Spell");
    }    

    public override void SetIsMoving(bool isMoving)
    {
        base.SetIsMoving(isMoving);
        animator.SetBool("isMoving", isMoving);
    }
}
