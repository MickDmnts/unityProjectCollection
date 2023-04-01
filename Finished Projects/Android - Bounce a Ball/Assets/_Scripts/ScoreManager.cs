using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager S;

    [Header("Set dynamically")]
    [SerializeField] Text scoreUiText;
    [SerializeField] int currentScore;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        S = this;
    }

    private void Start()
    {
        SetScoreTextReference();
        SetScoreToZeroAndApply();
    }

    private void SetScoreTextReference()
    {
        scoreUiText = GetComponent<Text>();
    }

    private void SetScoreToZeroAndApply()
    {
        currentScore = 0;
        ApplyScoreToTextElement(currentScore);
    }

    public void IncreaseScoreAndApply() //Gets called from BallBehaviour script
    {
        currentScore += 1;
        ApplyScoreToTextElement(currentScore);
    }

    void ApplyScoreToTextElement(int scoreToApply)
    {
        scoreUiText.text = currentScore.ToString();
    }
}
