using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite mouseOverSprite;

    private Sprite previousSprite;
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        previousSprite = buttonImage.sprite;
        buttonImage.sprite = mouseOverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = previousSprite;
    }
}
