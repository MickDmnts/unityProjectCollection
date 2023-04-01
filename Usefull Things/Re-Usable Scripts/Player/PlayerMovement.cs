using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRB;
    public float moveSpeed = 5f;
    
    Vector2 movement;

    void Awake() 
    {
        playerRB = playerRB.GetComponent<Rigidbody>();
    }

    void Start() 
    {
        playerRB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {
        playerRB.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
