using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Gradient colorGradient;

    [Header("Set Dynamically")]
    public Slider healthSlider;
    public Image barFill;

    private void Awake()
    {
        healthSlider = this.GetComponent<Slider>();
        barFill = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void SetMaxHealth(int value)
    {
        healthSlider.maxValue = value;
        healthSlider.value = value;

        barFill.color = colorGradient.Evaluate(1f);
    }

    public void SetHealth(int value)
    {
        healthSlider.value = value;

        barFill.color = colorGradient.Evaluate(healthSlider.normalizedValue);
    }
}
