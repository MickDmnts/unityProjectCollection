using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody PlayerRigidbody;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    [Range(1, 10)]
    public int jumpVelocity;
    public int maxJumps = 1;

    private int jumpCount = 0;

    void Awake() {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void Start() {
        jumpCount = maxJumps;
    }

    void Update() {
        if (Input.GetButtonDown("Jump") )
        {
            if (jumpCount > 0)
            {
                PlayerRigidbody.velocity = Vector3.up * jumpVelocity;
                jumpCount--;
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "ground")
        {
            jumpCount = maxJumps;
        }
    }

    void LateUpdate() {

        if (PlayerRigidbody.velocity.y < 0) //falling
        {
            PlayerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (PlayerRigidbody.velocity.y > 0 && !Input.GetButton("Jump")) //lowJump part
        {
            PlayerRigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
