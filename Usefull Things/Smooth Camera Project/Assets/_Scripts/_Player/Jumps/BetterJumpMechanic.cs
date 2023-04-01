using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpMechanic : MonoBehaviour
{
    //BetterJump mechanics
    [Header("Set in Inspector")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Set Dynamically")]
    [SerializeField]
    Rigidbody playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (playerRb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
