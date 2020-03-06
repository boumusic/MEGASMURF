using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUnitAnimator : UnitAnimator
{
    [Header("Components")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator[] armsAnimator;

    [SerializeField] private GameObject face;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject arms;

    private List<Animator> allAnimators = new List<Animator>();

    private void Start()
    {
        allAnimators.Add(bodyAnimator);
        for (int i = 0; i < armsAnimator.Length; i++)
        {
            allAnimators.Add(armsAnimator[i]);
        }
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
            allAnimators[i].SetBool("isMoving", isMoving);
        }
    }

}
