using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderUnitMerge : MonoBehaviour
{
    public ShapeUnit toMergeOn;
    [SerializeField] private AnimationCurve curveHoriz;
    [SerializeField] private AnimationCurve curveVerti;
    [SerializeField] private AnimationCurve curveRot;
    [SerializeField] private float maxY = 2f;
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private float speed = 1f;
    private bool isMerging = false;
    private float mergeProgress = 0f;
    private Vector3 initialPos;
    private Vector3 init;

    void Start()
    {
        init = transform.position;

    }
    private void Update()
    {
        MergeUpdate();
        if(Input.GetKeyDown(KeyCode.R))
        {
            Merge();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = init;
        }
    }

    private void Merge()
    {
        if (toMergeOn)
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
                Vector3 pos = Vector3.Lerp(initialPos, toMergeOn.transform.position, curveHoriz.Evaluate(mergeProgress));
                float y = Mathf.LerpUnclamped(initialPos.y, toMergeOn.transform.position.y + offsetY, curveVerti.Evaluate(mergeProgress));
                float xRot = curveRot.Evaluate(mergeProgress) * 360f;
                transform.position = new Vector3(pos.x, y, pos.z);
                transform.localEulerAngles = new Vector3(xRot, 0f, 0f);

                if(mergeProgress > 0.7f)
                {
                    toMergeOn.GetComponentInChildren<Animator>().SetBool("isLandedOn", true);
                }
            }

            else
            {
                    toMergeOn.GetComponentInChildren<Animator>().SetBool("isLandedOn", false);
                isMerging = false;
            }
        }
    }
}
