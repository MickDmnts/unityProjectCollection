using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMonitor : MonoBehaviour
{
    public FloatVariable health;
    Text healthText;

    private void Start()
    {
        healthText = GetComponent<Text>();
    }

    private void Update()
    {
        SetHealthText();
    }

    private void SetHealthText()
    {
        float healthValue = health.RuntimeValue;
        healthText.text = healthValue.ToString();
    }
}
