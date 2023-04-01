using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {
        CubeSceneEventSystem.S.onEnemyHit += Damage; //Subscribe to the event system
    }

    void Damage(Color newColor) => GetComponent<SpriteRenderer>().color = newColor;

    void OnDisable() => CubeSceneEventSystem.S.onEnemyHit -= Damage; //Unsbscribe to the event system
}
