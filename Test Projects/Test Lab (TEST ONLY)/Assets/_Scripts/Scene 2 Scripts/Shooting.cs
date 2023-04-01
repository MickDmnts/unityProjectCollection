using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("What to shoot")]
    [Space]
    public Rigidbody bullet;
    [Header("Bullet Spawn")]
    [Space]
    public Transform spawner;
    [Header("Bullet Speed")]
    [Space]
    public float shootSpeed;

    void Update()
    {
        ShootBullet();
    }

    void ShootBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Single Press
        {
            Rigidbody newBullet = Instantiate(bullet, spawner.position, spawner.rotation) as Rigidbody;
            newBullet.AddForce(spawner.forward * shootSpeed, ForceMode.Force);
        }
    }
}
