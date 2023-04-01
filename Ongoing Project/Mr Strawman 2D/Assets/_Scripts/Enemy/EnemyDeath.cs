using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    EnemyHealth enemyHealthScript;

    private void Start()
    {
        enemyHealthScript = GetComponent<EnemyHealth>();
    }

    private void LateUpdate()
    {
        if (CheckHealth())
            Destroy(gameObject);
    }

    private bool CheckHealth()
    {
        if (enemyHealthScript.EnemyHealthValue <= 0f)
            return true;
        return false;
    }
}
