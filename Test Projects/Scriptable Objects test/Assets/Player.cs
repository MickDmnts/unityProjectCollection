using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FloatVariable playerHealth;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DamagePlayer(1);
            Debug.Log(playerHealth.RuntimeValue.ToString());
            CheckIfPlayerDead();
        }
    }

    private void DamagePlayer(float v) => playerHealth.SubtractValue(v);

    private bool CheckIfPlayerDead()
    {
        return playerHealth.RuntimeValue <= 0 ? true : false;
    }
}
