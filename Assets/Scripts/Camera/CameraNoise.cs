using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoiseAxis
{
    public bool enabled;
    [Range(0f, 0.01f)] public float speed;
    [Range(0f, 2f)] public float intensity;
}

public class CameraNoise : MonoBehaviour
{
    [Header("Settings")]
    public NoiseAxis xAxis;
    public NoiseAxis yAxis;
    public NoiseAxis zAxis;

    public GameObject go;

    private void Start()
    {
        if (!go) go = gameObject;
    }

    private void Update()
    {
        ApplyNoise();
    }

    private void ApplyNoise()
    {
        float x = IntensityAxis(xAxis);
        float y = IntensityAxis(yAxis);
        float z = IntensityAxis(zAxis);
        Vector3 pos = new Vector3(x, y, z);
        go.transform.localPosition = pos;
    }

    private float IntensityAxis(NoiseAxis axis)
    {
        float output = axis.enabled ? Mathf.Sin(Time.frameCount * axis.speed) * axis.intensity : 0f;
        return output;
    }
}
