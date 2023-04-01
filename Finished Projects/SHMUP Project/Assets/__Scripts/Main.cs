using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = .5f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck bndsCheck;

    private void Awake()
    {
        S = this;
        bndsCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        //Instantiate a random Enemy Ship
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Position the enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null) //If the GO has a BoundsCheck attached, take its radius instead.
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set the initial position for the spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndsCheck.camWidth + enemyPadding;
        float xMax = bndsCheck.camWidth + enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndsCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //Call the SpawnEnemy function again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    private void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
