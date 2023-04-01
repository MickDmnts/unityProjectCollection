using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondCamera;

    void Start()
    {
        mainCamera = mainCamera.GetComponent<Camera>();
        secondCamera = secondCamera.GetComponent<Camera>();
        secondCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeCameras();
        }
    }

    void ChangeCameras()
    {
        if (mainCamera.enabled)
        {
            secondCamera.enabled = !secondCamera.enabled;
        }
    }
}
