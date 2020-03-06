using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(0, 3), SerializeField] private float timeScale;
    public bool toggle;

    // Update is called once per frame
    void Update()
    {
        if (toggle) Time.timeScale = timeScale;   
    }
}
