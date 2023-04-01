using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    
    Transform shootingPoint;
    Transform bulletAnchor;

    private void Awake()
    {
        shootingPoint = gameObject.transform.GetChild(0).transform;
        bulletAnchor = GameObject.FindGameObjectWithTag("BulletAnchor").transform;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootingPoint.up * bulletSpeed, ForceMode2D.Impulse);
        bullet.transform.SetParent(bulletAnchor);
    }
}
