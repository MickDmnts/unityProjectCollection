using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private Rigidbody rb;

    public float speed;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() //Make the sphere move around
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hor, 0.0f, ver);

        rb.AddForce(movement * speed);
    }
}
