using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToTransform : MonoBehaviour
{
    public Transform toSnapTo;
    public Vector3 rotOffset;

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
            transform.eulerAngles = toSnapTo.eulerAngles + rotOffset;
        }
    }
}
