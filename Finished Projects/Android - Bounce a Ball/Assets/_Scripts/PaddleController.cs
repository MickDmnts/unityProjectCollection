using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Set dynamically")]
    [SerializeField]Rigidbody2D paddleRb;

    [Header("Set in inspector")]
    public float paddleStrafeSpeed = 0.1f;

    private void Awake()
    {
        AssignReferences(); 
    }

    private void AssignReferences()
    {
        paddleRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            MovePaddle();
        }
        else
        {
            paddleRb.velocity = Vector2.zero;
        }
    }

    void MovePaddle()
    {
        Vector2 touchedPos = GetTouchedPosWorldCoordinates(Input.mousePosition);
        if (touchedPos.x < 0)
        {
            paddleRb.velocity = Vector2.left * paddleStrafeSpeed;
        }
        else
        {
            paddleRb.velocity = Vector2.right * paddleStrafeSpeed;
        }
    }

    Vector2 GetTouchedPosWorldCoordinates(Vector2 touchedPos)
    {
        return Camera.main.ScreenToWorldPoint(touchedPos);
    }
}
