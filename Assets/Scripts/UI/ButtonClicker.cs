using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicker : MonoBehaviour
{
    public void Click()
    {
        GetComponent<UIButton>().OnClick();
    }
}
