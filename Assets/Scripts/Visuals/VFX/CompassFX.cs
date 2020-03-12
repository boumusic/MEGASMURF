using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassFX : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem startTracing;
    public ParticleSystem inPs;
    public ParticleSystem appear;
    public bool playOnStart = false;

    private void OnEnable()
    {
        if(playOnStart) Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Play();
        }
    }

    public void Play()
    {
        animator.SetTrigger("In");
        inPs.Play();
    }

    public void StartTracing()
    {
        startTracing.Play();
    }

    public void Appear()
    {
        appear.Play();
    }
}
