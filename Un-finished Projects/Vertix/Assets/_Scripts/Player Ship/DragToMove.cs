using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToMove : MonoBehaviour
{
    [Header("Set dynamically")]
    [SerializeField] GameObject _playerShipGO;

    private void Awake()
    {
        GetPlayerShipRef();
    }

    private void GetPlayerShipRef()
    {
        _playerShipGO = gameObject;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            SetPlayerShipXPos();
        }
    }

    private void SetPlayerShipXPos()
    {
        Vector3 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _playerShipGO.transform.position = new Vector2(mousePos2D.x, _playerShipGO.transform.position.y);
    }
}
