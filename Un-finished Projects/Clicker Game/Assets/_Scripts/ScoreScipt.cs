using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScipt : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreText;

    private int score;

    private void Awake()
    {
        score = 0;
        scoreText.text = "Clicks: " + score;
    }

    public void AddPoints()
    {
        score++;
        scoreText.text = "Clicks: " + score;
    }
}
