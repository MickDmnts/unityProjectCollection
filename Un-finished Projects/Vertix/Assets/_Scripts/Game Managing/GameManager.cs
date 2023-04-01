using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public void SetSingleton()
    {
        S = this;
    }

    private void Awake()
    {
        SetSingleton();
    }

    public void StartGame()
    {
        GameSceneManager.S.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
