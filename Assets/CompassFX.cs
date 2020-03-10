using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassFX : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem startTracing;
    public ParticleSystem appear;
    public bool playOnStart = false;

    private void Start()
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
        appear.Play();
    }

    public void StartTracing()
    {
        startTracing.Play();
    }
}
