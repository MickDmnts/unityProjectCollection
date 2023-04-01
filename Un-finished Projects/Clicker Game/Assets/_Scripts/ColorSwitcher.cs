using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{
    [Header("Set Dynamically")]
    public SpriteRenderer clickArea;

    private Color green;
    private Color red;

    private void Awake()
    {
        clickArea = GetComponent<SpriteRenderer>();
        clickArea.color = new Color(255f, 0f, 0f); //Set the click area to red as default

        //Set the colors to lerp
        green = new Color(0f, 255f, 0f);
        red = new Color(255f, 0f, 0f);
    }

    private void OnMouseEnter()
    {
        clickArea.color = green; //Green
    }

    private void OnMouseExit()
    {
        clickArea.color = red; //Red
    }

    private void OnMouseDown()
    {
        clickArea.color = new Color(clickArea.color.r, clickArea.color.g, clickArea.color.b, 0.80f);
    }
}
