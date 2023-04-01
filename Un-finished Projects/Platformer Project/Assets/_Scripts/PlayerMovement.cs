using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRB;
    public Transform playerT;
    public float playerSpeed = 5f;

    void Awake() {
        playerT = playerT.GetComponent<Transform>();
        playerRB = playerRB.GetComponent<Rigidbody>();
    }

    void Start() {
        playerRB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update() {
        float moveHor = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(moveHor,0f,0f) * playerSpeed * Time.deltaTime; //Move mechanics
        playerT.transform.Translate(move);
    }
}
