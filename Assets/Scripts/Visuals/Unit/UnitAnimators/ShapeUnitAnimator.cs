using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

[System.Serializable]
public class OverridableAnimator
{
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClipOverrides clipOverrides;

    public void Initialize()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
    }

    public void Play(string name, AnimationClip clip)
    {
        clipOverrides[name] = clip;
        animator.SetTrigger(name);
    }
}

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
    
    private void Start()
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

    public void PlaySpecial(string name)
    {
        ShapeUnitAnimation anim = list?.GetUnitAnimation(name);
        if(anim != null)
        {
            legAnimator.Play(name, anim.legs);
            for (int i = 0; i < armsAnimator.Length; i++)
            {
                armsAnimator[i].Play(name, anim.arms);
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
