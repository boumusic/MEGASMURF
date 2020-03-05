using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pataya.QuikFeedback;

public class UnitAnimator : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private QuikFeedback[] feedbacks;

    public virtual void SetIsMoving(bool isMoving)
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
