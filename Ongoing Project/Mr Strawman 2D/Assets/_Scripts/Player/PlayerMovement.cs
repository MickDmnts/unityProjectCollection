using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float playerSpeed = 5f;
    
    Rigidbody2D playerRB;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    float h, v;
    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {   
        MovePlayer(h, v);
    }

    Vector3 movement;
    private void MovePlayer(float h, float v)
    {
        movement.Set(h, v, 0f);
        movement = movement.normalized * playerSpeed * Time.deltaTime;
        playerRB.MovePosition(transform.position + movement);
    }
}
