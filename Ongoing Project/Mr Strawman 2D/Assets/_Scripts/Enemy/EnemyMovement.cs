using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemyRB;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    public float enemySpeed;
    public Vector2 movement;
    public Vector3 playerPos;

    private void MoveEnemy()
    {
        playerPos = PlayerInstance.S.GetPlayerPos();


        /*float speed = enemySpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed);*/
    }
}
