using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_3")]
    public Vector3[] points;
    public float birthTime;

    private void Start()
    {
        points = new Vector3[3]; //Initialize the points array;
        points[0] = pos; //Start position is already set by Main.SpawnEnemy();

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        //Pick a random middle position in the bottom half of the screen;
        Vector3 v;
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        //Pick a random final position above the top of the screen
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = pos.y;
        points[2] = v;

        //Set birthTime to the current time
        birthTime = Time.time;
    }

    public override void Move()
    {
        //Bezier curves work based on a U value between 0 & 1;
        float U = (Time.time - birthTime) / lifeTime;
        
        if (U >= 1)
        {
            //This GO has reached its final position;
            Destroy(this.gameObject);
            return;
        }

        //Interpolate the three Bezier curve points;
        Vector3 p01, p12;
        //U = U - 0.2f * Mathf.Sin(U * Mathf.PI * 2); //Adds a bit of easing to the moving of the Enemy_3;
        p01 = (1 - U) * points[0] + U * points[1];
        p12 = (1 - U) * points[1] + U * points[2];
        Vector3 p012 = (1 - U) * p01 + U * p12;
        pos = p012;
    }
}
