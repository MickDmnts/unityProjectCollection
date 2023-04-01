using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    public static BoundsChecker S;

    [Header("Set in Inspector")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _playerShip;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        S = this;
    }

    public bool CheckBounds()
    {
        Vector2 shipPos = _playerShip.transform.position;
        float camWidth = GetCameraWidth();

        if (shipPos.x > camWidth - .3f)
        {
            return true;
        }
        else if (shipPos.x < -camWidth + .3f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    float GetCameraWidth()
    {
        float camHeight = _mainCamera.orthographicSize;
        float camWidth = camHeight * _mainCamera.aspect;
        return camWidth;
    }
}
