using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager S; //Singleton

    [Header("Player specific - Set in inspector")]
    public Transform playerPrefab;
    public Transform playerSpawnPoint;
    public int spawnDelay = 2;

    private void Start()
    {
        CheckSingleton();
        CheckIfPrefabIsSet();
        CheckIfSpawnPointIsSet();
    }

    void CheckSingleton()
    {
        if (S == null)
        {
            S = this;
        }
    }

    void CheckIfPrefabIsSet()
    {
        if (playerPrefab == null)
        {
            Debug.Log("Player prefab is not set");
        }
    }

    void CheckIfSpawnPointIsSet()
    {
        if (playerSpawnPoint == null)
        {
            Debug.Log("Player spawn point is not set");
        }
    }

    public void DestroyPlayer(Player playerRef)
    {
        Destroy(playerRef.gameObject);
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSecondsRealtime(spawnDelay);
        Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
    }
}
