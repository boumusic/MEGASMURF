using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.005f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField, Range(-1,1)] private float offset = 0f;
    
    private void Update()
    {
        float sin = Mathf.Sin(Time.frameCount * speed + offset * Mathf.PI) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, sin, transform.localPosition.z);
    }
}
