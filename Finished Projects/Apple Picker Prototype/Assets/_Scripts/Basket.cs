using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Basket : MonoBehaviour
{
    public static Basket S;

    [Header("Set Dynamically")]
    public Text scoreGT;

    private void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounterText");
        scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = "0";
        S = this;
    }

    private void Update()
    {
        Vector3 mousePos2D = Input.mousePosition; //Get the position of the mouse in the screen      

        mousePos2D.z = -Camera.main.transform.position.z; //The Camera's Z positon sets how far to push the mouse into 3D

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D); //Convert the 2D mouse point to 3D world space

        Vector3 pos = transform.position;
        pos.x = mousePos3D.x;
        transform.position = pos;

    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject appleToDestroy = coll.gameObject;
        if (appleToDestroy.tag == "Apple")
        {
            Destroy(appleToDestroy);
            int score = int.Parse(scoreGT.text); //Convert the string to int
            score += 100;
            scoreGT.text = score.ToString();

            if (score > HighScoreController.highScore)
            {
                HighScoreController.highScore = score;
            }
        }
    }
}
