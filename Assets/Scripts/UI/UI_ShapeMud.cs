using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ShapeMud : MonoBehaviour
{

    public TextMeshProUGUI ShapemudValueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShapemudValueText.text = GameManager.ShapeMud.ToString();
    }
}
