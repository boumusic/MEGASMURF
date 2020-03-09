using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pataya.QuikFeedback;

public class Jauge : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private QuikFeedback lose;

    [SerializeField] private float smoothness = 0.1f;
    private float current = -1f;
    private float max = 10f;

    private float currentVel;

    public void UpdateJauge(float current, float max)
    {
        if(current < this.current)
        {
            lose.Play();
        }

        this.current = current;
        this.max = max;
    }

    private void Update()
    {
        float target = current / max;
        float newFill = Mathf.SmoothDamp(fill.fillAmount, target, ref currentVel, smoothness);
        text.text = current + "/" + max;
        fill.fillAmount = newFill;
    }
}
