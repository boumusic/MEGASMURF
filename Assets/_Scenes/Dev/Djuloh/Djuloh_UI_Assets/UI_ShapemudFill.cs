using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_ShapemudFill : UIElement
{
    [Header("Debug")]
    private float lerpSpeed = 3f;
    public int ShapemudValue, oldShapemudValue;
    public bool launchLerp;

    [Header("References")]
    public Image ShapemudFill;
    public TextMeshProUGUI ShapemudValueText;

    private void Start()
    {
        ShapemudFill = gameObject.transform.GetChild(0).GetComponent<Image>();
        ShapemudValueText = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        
    }
    public void UpdateShapemudText(int newShapemudValue)
    {
        oldShapemudValue = ShapemudValue;
        ShapemudValue = newShapemudValue;
        

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
                lerpSpeed = 3f;
            }
        } 

    }

  
}
