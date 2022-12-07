using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _enemyHealthValue;

    private void Start()
    {
        if (_enemyHealthValue <= 0f) _enemyHealthValue = 1f;
        SetEnemyHealth(_enemyHealthValue);
    }

    public float EnemyHealthValue { get => _enemyHealthValue; }

    public void SetEnemyHealth(float value) => _enemyHealthValue = value;

    public void SubtractEnemyValue(float value) => _enemyHealthValue -= value;
}
