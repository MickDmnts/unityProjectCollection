using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerStates
{
    left,
    right,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] PlayerStates _currentMovementState;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        CurrentPlayerState = PlayerStates.idle;
    }

    private PlayerStates CurrentPlayerState
    {
        get { return _currentMovementState; }
        set { _currentMovementState = value; }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        PlayerStates tempState = CurrentPlayerState;

        if (tempState==PlayerStates.idle)
        {
            if (!ScoreManager.S.CheckIfCanScore())
                return;
            ScoreManager.S.AddPoint();
        }
    }
}
