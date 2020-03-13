using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite baseSprite;
    public Sprite mouseOverSprite;

    private Image buttonImage;
    private bool isOverMoused;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        isOverMoused = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = mouseOverSprite;
        isOverMoused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = baseSprite;
        isOverMoused = false;
    }

    public void UpdateSprites()
    {
        if(isOverMoused)
            buttonImage.sprite = mouseOverSprite;
        else
            buttonImage.sprite = baseSprite;
    }
}
