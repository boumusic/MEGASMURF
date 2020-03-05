using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMergeAnimator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShapeUnitAnimator shapeUnitAnimator;
    [SerializeField] private Transform pivot;

    [Header("Animation")]
    [SerializeField] private AnimationCurve curveHoriz;
    [SerializeField] private AnimationCurve curveVerti;
    [SerializeField] private AnimationCurve curveRot;
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private float speed = 1f;

    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float releaseZoomDelay = 1f;

    private bool isMerging = false;
    private float mergeProgress = 0f;
    private Vector3 initialPos;
    private Vector3 init;
    private ShapeUnit destination;

    private System.Action finishedMerging;

    void Start()
    {
        init = transform.position;
    }

    public void MergeOnTopOf(ShapeUnit _destination, System.Action _finishedMerging)
    {
        GameCamera.Instance.RequestZoomOn(transform, GameCamera.closeZoom);
        finishedMerging = _finishedMerging;
        destination = _destination;
        StartCoroutine(InitialDelay());
    }

    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        shapeUnitAnimator.ToggleLegs(false);
        Merge();
    }

    private void Update()
    {
        MergeUpdate();
    }

    private void Merge()
    {
        if (destination)
        {
            initialPos = transform.position;
            isMerging = true;
            mergeProgress = 0f;
        }
    }

    private void MergeUpdate()
    {
        if(isMerging)
        {
            if(mergeProgress < 1f)
            {
                mergeProgress += Time.deltaTime * speed;
                Vector3 pos = Vector3.Lerp(initialPos, destination.transform.position, curveHoriz.Evaluate(mergeProgress));
                float y = Mathf.LerpUnclamped(initialPos.y, destination.transform.position.y + destination.Height, curveVerti.Evaluate(mergeProgress));
                float xRot = curveRot.Evaluate(mergeProgress) * -360f;
                transform.position = new Vector3(pos.x, y, pos.z);
                pivot.transform.localEulerAngles = new Vector3(xRot, 0f, 0f);
            }

            else
            {
                FinishedMerging();
            }
        }
    }

    private void FinishedMerging()
    {
        isMerging = false;
        finishedMerging?.Invoke();
        pivot.transform.localEulerAngles = Vector3.zero;
        StartCoroutine(ReleasingZoom());
    }

    private IEnumerator ReleasingZoom()
    {
        yield return new WaitForSeconds(releaseZoomDelay);
        GameCamera.Instance.ReleaseZoom();
    }
}
