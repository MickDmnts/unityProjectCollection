using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager S;
    public int points = 0;
    public int livesLeft = 3;
    public Text lifeText;
    public Text pointsText;

    private void Start()
    {
        S = this;
        livesLeft = 3;
        lifeText.text = "Lives left: " + livesLeft.ToString();
        points = 0;
        pointsText.text = "Points: " + points.ToString();
    }

    public void DecreaseLives()
    {
        livesLeft -= 1;
        lifeText.text = "Lives left: "+livesLeft.ToString();
    }

    public void IncreasePoints()
    {
        points++;
        pointsText.text = "Points: "+points.ToString();
    }
}
