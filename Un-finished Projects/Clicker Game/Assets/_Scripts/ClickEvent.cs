using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    Transform playAreaTrans;
    ScoreScipt _scoreScript;

    private void Awake()
    {
        _scoreScript = GetComponent<ScoreScipt>();
        playAreaTrans = GetComponent<Transform>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playAreaTrans.localScale = new Vector2(-11f, 4.5f);
            _scoreScript.AddPoints();           
        }
        if (Input.GetMouseButtonUp(0))
        {
            playAreaTrans.localScale = new Vector2(-12f, 5f);
        }
    }
}
