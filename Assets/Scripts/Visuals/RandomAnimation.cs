using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    public Animator animator;
    public int min;
    public int max;

    public void Start()
    {
        float blend = Random.Range(min, max);
        animator.SetFloat("Blend", blend);
    }
}
