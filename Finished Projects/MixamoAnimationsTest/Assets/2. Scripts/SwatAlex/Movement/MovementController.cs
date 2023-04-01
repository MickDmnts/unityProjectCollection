using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: make the animation work correct and stop looping

public class MovementController : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;

    [Header("Set dynamically")]
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Animator animator;
    [SerializeField] private float horizontalInput = 0;
    [SerializeField] private float verticalInput = 0;
    [SerializeField] private Vector3 rotation = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        playerTrans = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (horizontalInput < 0 || horizontalInput > 0)
        {
            RotateAroundSelf();
        }

        if (verticalInput < 0 || verticalInput > 0)
        {
            Move();
        }
    }

    private void RotateAroundSelf()
    {

        rotation.y = rotationSpeed * horizontalInput * Time.deltaTime;
        playerTrans.Rotate(rotation);

        HandleRotateAnim();
    }

    private void Move()
    {
        playerTrans.position += playerTrans.forward * verticalInput * moveSpeed * Time.deltaTime;
    }

    //Animations
    private void HandleRotateAnim()
    {
        animator.SetBool(name: "canRotate", value: true);
        animator.SetFloat(name: "horizontalInput", value: horizontalInput);
    }
}
