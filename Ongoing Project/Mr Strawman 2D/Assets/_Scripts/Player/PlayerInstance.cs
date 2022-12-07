using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance S;

    static Rigidbody2D playerRB;

    private void Awake()
    {
        S = this;
        playerRB = GetComponent<Rigidbody2D>();
    }

    public Transform GetPlayerTransform() => playerRB.transform;

    public Vector3 GetPlayerPos() => playerRB.transform.position;

    public CircleCollider2D GetPlayerCollider() => GetComponent<CircleCollider2D>();
}
