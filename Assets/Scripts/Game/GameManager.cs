using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static int shapeMud;
    public static int ShapeMud 
    {
        get
        {
            return shapeMud;
        }
        set
        {
            shapeMud = value;
            UIManager.Instance.UpdateShapeMud(value);
        }
    }
    public static SkillTree SkillTree;
    public static List<Unit> units;

    public static void PayShapeMudCost(int cost)
    {
        GameManager.ShapeMud -= cost;
    }
}
