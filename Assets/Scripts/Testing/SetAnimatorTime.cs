using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorTime : MonoBehaviour
{
    public Animator animator;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            animator.Play("Default", -1, 0f);
        }
    }
}
