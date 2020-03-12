using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pataya.QuikFeedback;

public class ShapeAppear : MonoBehaviour
{
    public float appearDelay = 1.5f;

    public QuikFeedback appearAnticipation;
    public QuikFeedback appearClimax;

    private void OnEnable()
    {
        StartCoroutine(Appearing());
    }

    private IEnumerator Appearing()
    {
        appearAnticipation.Play();
        yield return new WaitForSeconds(appearDelay);
        DoAppear();
    }

    public void DoAppear()
    {
        appearClimax.Play();
    }
}
