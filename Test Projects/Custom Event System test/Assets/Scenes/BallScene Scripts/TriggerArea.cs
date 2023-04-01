using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        BallSceneEventSystem.S.ActionTriggerEnter();
        Debug.Log("ActionTriggerEnter() got called");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BallSceneEventSystem.S.ActionTriggerExit();
        Debug.Log("ActionTriggerExit() got called");
    }
}
