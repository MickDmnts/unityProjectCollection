using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffLight : MonoBehaviour
{
    public Light lightSphere;

    void Update()
    {
        LightChange();
    }

    void LightChange()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!lightSphere.enabled)
            {
                lightSphere.enabled = true;
            }
            else
            {
                lightSphere.enabled = false;
            }
        }
    }
}
