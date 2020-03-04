using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    private List<Brain> aIBrains;

    private void Awake()
    {
        instance = this;
    }

    public void Process()
    {
        
    }

    public void AddBrain(Brain brain)
    {
        aIBrains.Add(brain);
    }
}
