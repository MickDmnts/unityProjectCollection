using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSceneEventSystem : MonoBehaviour
{
    public static BallSceneEventSystem S;

    private void Awake()
    {
        S = this;
    }

    public event Action onTriggerActionEnter;
    public void ActionTriggerEnter()
    {
        onTriggerActionEnter?.Invoke(); //Sends the signal to all subs if not null
    }

    public event Action onTriggerActionExit;
    public void ActionTriggerExit()
    {
        onTriggerActionExit?.Invoke(); //Sends the signal to all subs if not null
    }
}