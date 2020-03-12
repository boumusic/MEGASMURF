using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUnitAnimator : UnitAnimator
{
    [Header("Components")]
    [SerializeField] private OverridableAnimator legAnimator;
    [SerializeField] private OverridableAnimator[] armsAnimator;

    [SerializeField] private GameObject face;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject arms;
    [SerializeField] private ShapeUnitAnimationsList list;

    private List<OverridableAnimator> allAnimators = new List<OverridableAnimator>();

    private void Awake()
    {
        allAnimators.Add(legAnimator);
        for (int i = 0; i < armsAnimator.Length; i++)
        {
            allAnimators.Add(armsAnimator[i]);
        }

        for (int i = 0; i < allAnimators.Count; i++)
        {
            allAnimators[i].Initialize();
        }
    }

    private void Start()
    {
        
    }

    public override void PlaySpecial(string name)
    {
        base.PlaySpecial(name);
        ShapeUnitAnimation anim = list?.GetUnitAnimation(name);
        if (anim != null)
        {
            legAnimator.Play("Special", anim.legs) ;
            for (int i = 0; i < armsAnimator.Length; i++)
            {
                armsAnimator[i].Play("Special", anim.arms);
            }
        }
    }

    public void ToggleLegAnimator(bool on)
    {
        legAnimator.animator.enabled = on;
    }

    public void ResetLegAnimator()
    {
        legAnimator.animator.Play("Idle", -1, 0f);
    }

    public void ToggleLegs(bool on)
    {
        legs.SetActive(on);
    }

    public void ToggleArms(bool on)
    {
        arms.SetActive(on);
    }

    public void ToggleFace(bool on)
    {
        face.SetActive(on);
    }

    public override void SetIsMoving(bool isMoving)
    {
        base.SetIsMoving(isMoving);

        for (int i = 0; i < allAnimators.Count; i++)
        {
            allAnimators[i].animator.SetBool("isMoving", isMoving);
        }
    }

}
