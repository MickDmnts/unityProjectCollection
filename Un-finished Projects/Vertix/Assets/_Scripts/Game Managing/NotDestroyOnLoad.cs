using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyOnLoad : MonoBehaviour
{
    public static NotDestroyOnLoad S;

    private void Awake()
    {
        CheckNullCondition();
    }

    public void SetSingleton()
    {
        S = this;
    }

    private void CheckNullCondition()
    {
        if (S != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SetSingleton();
            DontDestroyOnLoad(gameObject);
        }
    }

}
