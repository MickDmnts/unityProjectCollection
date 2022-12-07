using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private void Update()
    {
        Text tempScoreText = Basket.S.scoreGT;
        int currentScore = int.Parse(tempScoreText.text);

        if (currentScore == 2500)
        {
            
            print("Level2");
            AppleTree.S.secondsBetweenAppleDrops = 0.8f;
        }

        if (currentScore == 5500)
        {
            print("Level3");
            AppleTree.S.secondsBetweenAppleDrops = 0.7f;
        }

        if (currentScore == 9500)
        {
            print("Level4");
            AppleTree.S.secondsBetweenAppleDrops = 0.6f;
        }

        if (currentScore == 14000)
        {
            print("Level5");
            AppleTree.S.secondsBetweenAppleDrops = 0.5f;
        }
        
        if (currentScore == 20000)
        {
            print("Won");
            AppleTree.S.secondsBetweenAppleDrops = 0.4f;
        }
    }
}
