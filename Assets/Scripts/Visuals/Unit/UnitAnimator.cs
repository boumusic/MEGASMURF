using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pataya.QuikFeedback;

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
        if(animator)
        {
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);
        }
    }

    public void Play(string name, AnimationClip clip)
    {
        clipOverrides[name] = clip;
        animator.SetTrigger(name);
    }
}

public class UnitAnimator : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private QuikFeedback[] feedbacks;


    public virtual void SetIsMoving(bool isMoving)
    {

    }

    public virtual void PlaySpecial(string name)
    {
        
    }

    public void PlayFeedback(string name)
    {
        for (int i = 0; i < feedbacks.Length; i++)
        {
            if(feedbacks[i].feedbackName == name)
            {
                feedbacks[i].Play();
                return;
            }
        }
    }
}
