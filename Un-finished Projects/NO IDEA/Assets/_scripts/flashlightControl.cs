using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightControl : MonoBehaviour
{
    [SerializeField]
    AudioSource flashlightOn;

    [SerializeField]
    AudioSource flashlightOff;

    public Light flashlight;

    bool flashOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !flashOn)
        {
            flashlightOn.Play();
            flashlight.enabled = true;
            flashOn = !flashOn;
        }
        else if (Input.GetKeyDown(KeyCode.F) && flashOn)
        {
            flashlightOff.Play();
            flashlight.enabled = false;
            flashOn = !flashOn;
        }
    }
}
