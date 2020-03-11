using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_ShapemudFill : MonoBehaviour
{
    public float lerpSpeed = 4f;
    public float ShapemudValue, oldShapemudValue;
    private float FillAmount, oldFillAmount;
    private TextMeshProUGUI ShapemudValueText;
    public bool launchLerp;
    private Image ShapemudFill;

    private void Start()
    {
        ShapemudFill = gameObject.transform.GetChild(0).GetComponent<Image>();
        ShapemudValueText = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        oldShapemudValue = ShapemudValue;
       
        oldFillAmount = oldShapemudValue / 200f;
        FillAmount = ShapemudValue / 200f;
    }
    public void UpdateShapemudText(float newShapemudValue)
    {
        oldShapemudValue = ShapemudValue;
        ShapemudValue = newShapemudValue;
        oldFillAmount = oldShapemudValue / 200;
        FillAmount = ShapemudValue / 200;

        launchLerp = true;

    }
    private void FixedUpdate()
    {
        if (launchLerp)
        {
            
            Mathf.Lerp(oldShapemudValue, ShapemudValue, lerpSpeed * Time.deltaTime);
            Mathf.Lerp(oldFillAmount, FillAmount, lerpSpeed * Time.deltaTime);
            ShapemudFill.fillAmount = oldFillAmount;
            ShapemudValueText.text = oldShapemudValue.ToString();
            if (oldShapemudValue == ShapemudValue)
            {
                launchLerp = false;
            }
        } 

    }

  
}
