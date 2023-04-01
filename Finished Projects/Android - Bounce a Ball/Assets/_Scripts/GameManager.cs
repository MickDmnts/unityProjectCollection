using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager S;

    [Header("Set dynamically")]
    [SerializeField] float camHeight;
    [SerializeField] GameObject ballPrefab;

    [Header("Set in inspector")]
    public Scene currentScene;

    private void Awake()
    {
        SetSingleton();
        SetCurrentScene();
        GetCamHeight();
        GetBallPrefabReference();
    }

    void SetSingleton()
    {
        S = this;
    }

    private void SetCurrentScene()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void GetCamHeight()
    {
        camHeight = Camera.main.orthographicSize;
    }

    private void GetBallPrefabReference()
    {
        ballPrefab = GameObject.FindGameObjectWithTag("Ball");
    }

    private void FixedUpdate()
    {
        if (ballPrefab != null && ballPrefab.transform.position.y < -camHeight)
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
