using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in inspector: Enemy_2")]
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;
    public float YSpawnValue = 20;

    [Header("Set dynamically")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    private void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight + YSpawnValue, bndCheck.camHeight);

        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //A slight posibility to swap sides, setting the x to its negative would swap it
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move()
    {
        //Main
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //Interpolation
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        pos = (1 - u) * p0 + u * p1; //Main interpolation formulae

        base.Move();
    }
}
