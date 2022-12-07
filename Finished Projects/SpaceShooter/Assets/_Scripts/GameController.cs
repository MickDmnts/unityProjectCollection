using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class  GameController : MonoBehaviour
{
    public GameObject hazard; //What to spawn
    public Vector3 spawnValues; //Where to spawn hazards
    public int hazardSpawn; //How many hazards to spawn
    public float timeBetweenSpawns; //Time between hazards spawn
    public float startWait; //Time for the player to get ready
    public float waveSpawner; //Time between waves

    public Text scoreText; //Just the UI component
    private int count; //Non-interactive with inspector

    public Text restartText; //UI component
    public Text gameOverText; //UI component
    private bool gameOver; //True if player destroyed
    private bool restart; //True if gameOver is true


    void Start()
    {
        count = 0;
        UpdateScore();
        gameOver = false;
        restart = false;
        restartText.enabled = false;
        gameOverText.enabled = false;
        StartCoroutine(Spawner()); //Specific call to start a Coroutine
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    IEnumerator Spawner() //MUST BE IEnumerator to work - Coroutine
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardSpawn; i++)
            {
                Vector3 vecPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 0.0f, spawnValues.z);
                Quaternion quaRotation = Quaternion.identity;
                Instantiate(hazard, vecPosition, quaRotation);
                yield return new WaitForSeconds(timeBetweenSpawns);
                if (gameOver)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(waveSpawner);

            if (gameOver)
            {
                restartText.enabled = true;
                restart = true;
                break;
            }
        }
    }

    public void NewScore(int addedScore)
    {
        count += addedScore;
        UpdateScore();
    }

    public void GameOver()
    {
        gameOverText.enabled = true;
        gameOver = true;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + count;
    }
}
