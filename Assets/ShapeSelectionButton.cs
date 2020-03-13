using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSelectionButton : UIButton
{
    public BaseUnitType type;

    public override void OnClick()
    {
        base.OnClick();
        switch (type)
        {
            case BaseUnitType.Triangle:
                InputManager.instance.OnTriangleButtonPress();
                break;
            case BaseUnitType.Circle:
                InputManager.instance.OnCircleButtonPress();
                break;
            case BaseUnitType.Square:
                InputManager.instance.OnSquareButtonPress();
                break;
        }
    }

}
