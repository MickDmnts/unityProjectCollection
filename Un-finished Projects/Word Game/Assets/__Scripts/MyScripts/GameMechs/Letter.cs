using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class makes use of several properties to perform various actions when variables are set
/// for ex. It enables the WordGame to set the char c of a letter without worrying about how that gets converted to a string
/// </summary>
public class Letter : MonoBehaviour
{
    [Header("Set in inspector")]
    public float timeDuration = .5f;
    public string easingCurve = Easing.InOut; //Easing from Utils.cs

    [Header("Set Dynamically")]
    public TextMesh tMesh; //The TextMesh shows the char
    public Renderer tRend; //The Renderer of the 3D Text
    public bool big = false;
    public List<Vector3> pts = null;
    public float timeStart = -1;

    private char _c; //The char to be displayed
    private Renderer rend;

    private void Awake()
    {
        tMesh = GetComponentInChildren<TextMesh>();
        tRend = tMesh.GetComponent<Renderer>();
        rend = GetComponent<Renderer>();
        visible = false;
    }

    //Interpolation code
    private void Update()
    {
        if (timeStart == -1)
        {
            return;
        }

        //Standard linear interpolation
        float u = (Time.time - timeStart) / timeDuration;
        u = Mathf.Clamp01(u);

        float u1 = Easing.Ease(u, easingCurve);
        Vector3 v = Utils.Bezier(u1, pts);
        transform.position = v;

        //If the interpolation is done, set timeStart back to -1;
        if (u == 1) timeStart = -1;
    }

    /// <summary>
    /// Property to get or set the character to be displayed
    /// </summary>
    public char c
    {
        get
        {
            return _c;
        }
        set
        {
            _c = value;
            tMesh.text = _c.ToString();
        }
    }
    
    /// <summary>
    /// Gets or sets _c as a String
    /// </summary>
    public string str
    {
        get
        {
            return _c.ToString();
        }
        set
        {
            _c = value[0]; // Gets only the first char of the sentence
        }
    }

    /// <summary>
    /// Returns the state or sets the visibility of the letter
    /// </summary>
    public bool visible
    {
        get
        {
            return tRend.enabled;
        }
        set
        {
            tRend.enabled = value;
        }
    }

    /// <summary>
    /// Gets or sets the color of the rounded rectangle
    /// </summary>
    public Color color
    {
        get
        {
            return rend.material.color;
        }
        set
        {
            rend.material.color = value;
        }
    }

    /// <summary>
    /// Now sets up a Bezier curve to move to the new position
    /// </summary>
    public Vector3 pos
    {
        set
        {
            //Find a midpoint that is a random distance from the actual
            // midpoint between the current position and the value passed in
            Vector3 mid = (transform.position + value) / 2f;

            //The random distance will be within 1/4 of the magnitude of the
            // line from the actual midpoint
            float mag = (transform.position - value).magnitude;
            mid += Random.insideUnitSphere * mag * .25f;

            //Create a List<Vector3> of Bezier points
            pts = new List<Vector3>() { transform.position, mid, value };

            //If timeStart is at default -1, then set it
            if (timeStart == -1)
            {
                timeStart = Time.time;
            }
        }
    }

    //Moves immediately to the new position
    public Vector3 posImmediate
    {
        set
        {
            transform.position = value;
        }
    }
}
