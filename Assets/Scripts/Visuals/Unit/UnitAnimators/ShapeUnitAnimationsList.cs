using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShapeUnitAnimation
{
    public string name;
    public AnimationClip arms;
    public AnimationClip legs;
}

public class ShapeUnitAnimationsList : MonoBehaviour
{
    [Header("Special Animations")]
    public List<ShapeUnitAnimation> animations = new List<ShapeUnitAnimation>();
    private Dictionary<string, ShapeUnitAnimation> animDict = new Dictionary<string, ShapeUnitAnimation>();
    
    private void Awake()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            animDict.Add(animations[i].name, animations[i]);
        }
    }

    public ShapeUnitAnimation GetUnitAnimation(string name)
    {
        return animDict[name];
    }
}
