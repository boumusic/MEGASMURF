using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleClearer : MonoBehaviour
{
    public float delay = 1.5f;
    public ParticleSystem ps;
    public ParticleSystem toClear;

    public bool debug;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && debug)
        {
            Play();
        }
    }

    public void Play()
    {
        ps.Play();
        StartCoroutine(Clearing());
    }

    private IEnumerator Clearing()
    {
        yield return new WaitForSeconds(delay);
        toClear.Clear(false);
    }
}
