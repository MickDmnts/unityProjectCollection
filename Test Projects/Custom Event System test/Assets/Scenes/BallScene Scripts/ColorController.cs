using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorController : MonoBehaviour
{
    private void Start()
    {
        BallSceneEventSystem.S.onTriggerActionEnter += SetToGreen; //Subscribe the event to the GameEventSystem
        BallSceneEventSystem.S.onTriggerActionExit += SetToRed;
    }

    private void SetToGreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.green;
    }

    private void SetToRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }
}
