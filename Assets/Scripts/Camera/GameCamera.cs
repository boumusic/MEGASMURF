﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private static GameCamera instance;
    public static GameCamera Instance { get { if (!instance) instance = FindObjectOfType<GameCamera>(); return instance; } }

    public static float microZoom = -100;
    public static float closeZoom = -200;
    public static float mediumZoom = -300;

    [SerializeField] private Camera cam;

    [Header("Zoom")]
    [SerializeField] private float smoothZoom;
    private float currentVelZoom;

    public Vector3 Forward => cam.transform.forward;
    private bool isZoomedIn => target != null;
    private Transform target;
    private Vector3 initialPos;
    private Vector3 currentVelPos;
    private float zoom;

    private float initialZoom;
    
    private void Start()
    {
        initialZoom = cam.transform.localPosition.z;
        initialPos = transform.position;
        zoom = initialZoom;
    }

    public void RequestZoomOn(Transform _target, float _zoom)
    {
        if (!isZoomedIn)
        {
            target = _target;
            zoom = _zoom;
        }
    }

    public void ReleaseZoom()
    {
        zoom = initialZoom;
        target = null;
    }

    private void Update()
    {
        float desiredZoom = Mathf.SmoothDamp(cam.transform.localPosition.z, zoom, ref currentVelZoom, smoothZoom);
        Vector3 targetPos = new Vector3();

        if (target)
            targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
        else
            targetPos = initialPos;

        Vector3 desiredPos = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelPos, smoothZoom);
        //cam.orthographicSize = desiredZoom;
        cam.transform.localPosition = new Vector3(0, 0, desiredZoom);
        transform.position = desiredPos;
    }
}
