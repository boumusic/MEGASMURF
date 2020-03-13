using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pataya.QuikFeedback;

public class ShapeAppear : MonoBehaviour
{
    public float appearDelay = 1.5f;

    public QuikFeedback appearAnticipation;
    public QuikFeedback appearClimax;
    public CompassFX compassFx;
    public bool playOnEnable = false;

    public void OnEnable()
    {
        GetComponent<Unit>().hp.gameObject.SetActive(false);
        if(playOnEnable)
        {
            Appear();
        }
    }

    public void Appear()
    {
        StartCoroutine(Sound());
        StartCoroutine(Appearing());
        compassFx?.Play();
    }

    private IEnumerator Sound()
    {
        yield return new WaitForSeconds(0.7f);
        AudioManager.Instance.PlaySFX("ShapeSpawn_01");
    }

    private IEnumerator Appearing()
    {
        appearAnticipation.Play();
        yield return new WaitForSeconds(appearDelay);
        GetComponent<Unit>().hp.gameObject.SetActive(true);
        DoAppear();
    }

    public void DoAppear()
    {
        appearClimax.Play();
    }
}
