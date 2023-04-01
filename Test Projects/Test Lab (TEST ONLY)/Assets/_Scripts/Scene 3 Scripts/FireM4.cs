using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireM4 : MonoBehaviour
{
    public Transform spawner;
    public Rigidbody bullet;
    public float fireSpeed;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootM4();
        }
    }

    void ShootM4()
    {
        Rigidbody newBullet;
        newBullet = Instantiate(bullet, spawner.position, spawner.rotation) as Rigidbody;
        newBullet.AddForce(spawner.forward * fireSpeed);
    }
}
