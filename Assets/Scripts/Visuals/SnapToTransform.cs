using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToTransform : MonoBehaviour
{
    public Transform toSnapTo;
    public float rotationOffset = 0f;

    private void OnDrawGizmos()
    {
        UpdatePos();
    }

    private void Update()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (toSnapTo)
        {
            transform.position = toSnapTo.position;
            transform.rotation = toSnapTo.rotation * Quaternion.AngleAxis(rotationOffset, Vector3.right);
        }
    }
}
