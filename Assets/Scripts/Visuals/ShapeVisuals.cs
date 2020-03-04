using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeMember
{
    Face,
    Leg,
    Arm
}

public class ShapeVisuals : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator rootAnimator;
    [SerializeField] private Animator[] armsAnimators;
    [SerializeField] private Animator faceAnimator;

    private List<Animator> allAnimators = new List<Animator>();

    public void ToggleMember(bool on, ShapeMember member)
    {
        switch (member)
        {
            case ShapeMember.Face:
                faceAnimator.SetBool("on", on);
                break;

            case ShapeMember.Leg:
                rootAnimator.SetBool("on", on);
                break;

            case ShapeMember.Arm:
                for (int i = 0; i < armsAnimators.Length; i++)
                {
                    armsAnimators[i].SetBool("on", on);

                }
                break;
        }
    }
}
