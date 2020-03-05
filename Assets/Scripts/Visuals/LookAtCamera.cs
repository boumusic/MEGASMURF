using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Quaternion rotation;

    private void Start()
    {
        LookAtUpdate();
    }

    private void LookAtUpdate()
    {
        Camera cam = Camera.main;
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        Vector3 toLookAt = transform.position - cam.transform.forward * dist;

        transform.LookAt(toLookAt);
        transform.forward *= -1;

        rotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = rotation;
    }
}
