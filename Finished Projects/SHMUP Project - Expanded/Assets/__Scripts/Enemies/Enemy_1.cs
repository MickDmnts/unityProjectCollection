using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy 1")]
    public float waveFrequency = 2;
    //Sine wave width in meters
    public float waveWidth = 4;
    public float waveRotY = 45;

    //privates
    private float x0; //The initial x value of the pos
    private float birthTime; //Time.time - birthTime = age

    //Start works well cause it's not taken from the base.Enemy class
    private void Start()
    {
        x0 = pos.x;
        birthTime = Time.time;
    }

    public override void Move()
    {
        //Cause pos is a property you can't set pos.x directly
        //so get the pos as an editable Vector3
        Vector3 temp = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency; //theta changes based on Time
        float sin = Mathf.Sin(theta);
        temp.x = x0 + waveWidth * sin;
        pos = temp;

        //rotate a bit on Y axis
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
}
