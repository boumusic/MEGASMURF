using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float speed = 1f;

    private Vector3 initPos;

    private void Start()
    {
        initPos = transform.localPosition;
    }

    private void Update()
    {
        float sin = Mathf.Sin(Time.frameCount * speed + (initPos.x+initPos.y + initPos.z)) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, sin, transform.localPosition.z);
    }
}
