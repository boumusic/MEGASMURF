using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public ParticleSystem leftStep;
    public ParticleSystem rightStep;
    public void Step(int side)
    {
        int max = 4;
        int toPlay = Random.Range(0, max+1);

        string name = "Walking_0" + toPlay.ToString();
        AudioManager.Instance.PlaySFX(name);

        if (side == 0) leftStep.Play();
        else rightStep.Play();
    }
}
