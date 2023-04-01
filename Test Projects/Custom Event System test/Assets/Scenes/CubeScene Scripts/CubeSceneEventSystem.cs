using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSceneEventSystem : MonoBehaviour
{
    public static CubeSceneEventSystem S;

    private void Awake()
    {
        S = this;
    }

    public delegate void ChangeEnemyColor(Color newColor);
    public event ChangeEnemyColor onEnemyHit;

    public void HitEnemy(Color color)
    {
        onEnemyHit?.Invoke(color);
    }
}
