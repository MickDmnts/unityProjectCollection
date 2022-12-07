using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    Rigidbody2D playerRB;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    Vector2 mousePos;
    private void Update()
    {
        if (playerRB == null) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - playerRB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        playerRB.rotation = angle;
    }
}