using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    static public int highScore = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScoreText"))  //The GO's name
        {
            highScore = PlayerPrefs.GetInt("HighScoreText");
        }

        PlayerPrefs.SetInt("HighScoreText", highScore);
    }

    private void Update()
    {
        Text gt = GetComponent<Text>();
        gt.text = "High Score: " + highScore;
        if (highScore > PlayerPrefs.GetInt("HighScoreText"))
        {
            PlayerPrefs.SetInt("HighScoreText", highScore);
        }
    }
}
