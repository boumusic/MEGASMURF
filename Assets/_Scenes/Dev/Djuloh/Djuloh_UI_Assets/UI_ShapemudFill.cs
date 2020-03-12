using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_ShapemudFill : UIElement
{
    public float lerpSpeed = 4f;
    public int ShapemudValue, oldShapemudValue;
    private float FillAmount, oldFillAmount;
    private TextMeshProUGUI ShapemudValueText;
    public bool launchLerp;
    private Image ShapemudFill;

    private void Start()
    {
        ShapemudFill = gameObject.transform.GetChild(0).GetComponent<Image>();
        ShapemudValueText = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        
    }
    public void UpdateShapemudText(int newShapemudValue)
    {
        oldShapemudValue = ShapemudValue;
        ShapemudValue = newShapemudValue;
        oldFillAmount = oldShapemudValue / 200f;
        FillAmount = ShapemudValue / 200f;

        launchLerp = true;

    }
    private void Update()
    {
        if (launchLerp)
        {
            
            oldShapemudValue = (int)Mathf.Lerp(oldShapemudValue, ShapemudValue, lerpSpeed * Time.deltaTime);

            ShapemudFill.fillAmount = Mathf.Lerp((float)oldShapemudValue / 200f, (float)ShapemudValue / 200f, lerpSpeed * Time.deltaTime);
            ShapemudValueText.text = oldShapemudValue.ToString();

            lerpSpeed++;
            if (oldShapemudValue == ShapemudValue)
            {
                launchLerp = false;
                lerpSpeed = 0;
            }
        } 

    }

  
}
