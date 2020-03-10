using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{

    public RectTransform rect;
    public Vector2 offset;
    public Vector2 speed;
    public Vector2 amplitude;

    private Vector3 init;

    // Start is called before the first frame update
    void Start()
    {
        if (rect != null)
        {
            init = rect.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rect != null)
        {
            Vector3 next = new Vector3(init.x + amplitude.x * (1 + Mathf.Sin(offset.x + speed.x * Time.time)), init.y + amplitude.y * (1 + Mathf.Sin(offset.y + speed.y * Time.time)), 0);
            rect.localPosition = next;// SetPositionAndRotation(next, Quaternion.identity);
        }
    }
}
