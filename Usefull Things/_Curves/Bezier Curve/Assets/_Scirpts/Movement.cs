using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 10.0f;

    public float speed;

    public float velocity;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movement * speed);
        }

        velocity = rb.velocity.magnitude;
    }
}
