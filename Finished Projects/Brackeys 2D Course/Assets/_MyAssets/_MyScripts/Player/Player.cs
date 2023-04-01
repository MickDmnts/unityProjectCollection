using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public class PlayerStats
    {
        [SerializeField]
        int _health = 100;

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
    }

    [Header("PlayerStats reference - Set dynamically")]
    public PlayerStats playerStats;

    [Header("Player fall factor - Set in inspector")]
    public int fallFactor = -20;

    private void Start()
    {
        playerStats = new PlayerStats();
        CheckIfPlayerStatsRefIsSet();
    }

    void CheckIfPlayerStatsRefIsSet()
    {
        if (playerStats == null)
        {
            Debug.Log("PlayerStats reference is not set");
        }
    }

    private void Update()
    {
        if (IsPlayerFalling())
        {
            DamagePlayerAndCheckState(99999); 
        }
    }


    public void DamagePlayerAndCheckState(int damageGiven)
    {
        playerStats.Health -= damageGiven;
        if (IsPlayerDead())
        {
            GameManager.S.DestroyPlayer(this);
            GameManager.S.StartCoroutine(GameManager.S.RespawnPlayer());
        }
    }

    bool IsPlayerFalling()
    {
        if (transform.position.y <= fallFactor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsPlayerDead()
    {
        if (playerStats.Health <= 0 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
