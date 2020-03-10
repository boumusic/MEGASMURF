using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrolling : MonoBehaviour
{

    public RawImage image;
    public Vector2 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null)
        {
            Rect rect = new Rect();
            rect.Set(image.uvRect.x + speed.x * Time.deltaTime, image.uvRect.y + speed.y * Time.deltaTime, image.uvRect.width, image.uvRect.height);
            image.uvRect = rect;
        }
    }
}
