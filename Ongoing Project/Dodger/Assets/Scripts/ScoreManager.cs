using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager S; //Singleton

    [SerializeField] Text _scoreText;
    [SerializeField] int _currentScore;
    [SerializeField] int _scoreInterval = 1;
    [SerializeField] bool _canScore = true;

    void Start()
    {
        S = this;
        _scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        ZeroScore();
    }

    private void ZeroScore()
    {
        _currentScore = 0;
        _scoreText.text = "Score: " + _currentScore;
    }

    public void AddPoint()
    {
        _currentScore += 1;
        _scoreText.text = "Score: " + _currentScore;
    }

    public bool CheckIfCanScore()
    {
        float tempTime = Time.time;
        if (Time.time > tempTime + _scoreInterval)
        {
            _canScore = true;
            return _canScore;
        }
        else
        {
            _canScore = false;
            return _canScore;
        }
    }
}
