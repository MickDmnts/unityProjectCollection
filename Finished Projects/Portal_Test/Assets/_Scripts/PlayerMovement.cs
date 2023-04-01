using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalInput;
    [SerializeField] private Vector3 movement;

    private void Start()
    {
        playerTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        transform.position += playerTrans.right * horizontalInput * moveSpeed * Time.deltaTime;
    }
}
