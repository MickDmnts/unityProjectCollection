using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;

    [Header("Set in inspector")]
    public List<float> scoreFontSize = new List<float> { 36, 64, 64, 1 };
    public Vector3 scoreMidPoint = new Vector3(1, 1, 0);
    public float scoreTravelTime = 3f;
    public float scoreComboDelay = .5f;

    private RectTransform rectTrans;

    private void Awake()
    {
        S = this;
        rectTrans = GetComponent<RectTransform>();
    }

    /// <summary>
    /// This method allows ScoreManager.SCORE() to be called from anywhere
    /// </summary>
    /// <param name="wyrd"></param>
    /// <param name="combo"></param>
    static public void SCORE(Wyrd wyrd,int combo)
    {
        S.Score(wyrd, combo);
    }

    /// <summary>
    /// Add to the score for this wyrd
    /// Int combo is the number of this word in a combo
    /// </summary>
    /// <param name="wyrd"></param>
    /// <param name="combo"></param>
    void Score(Wyrd wyrd, int combo)
    {
        //Create a List<> of Vector2 Bezier points for the FloatingScore
        List<Vector2> pts = new List<Vector2>();

        //Get the position of the first Letter in the wyrd
        Vector3 pt = wyrd.letters[0].transform.position;
        pt = Camera.main.WorldToViewportPoint(pt);

        pts.Add(pt); //Make pt the first Bezier point

        //Add a second Bezier point
        pts.Add(scoreMidPoint);

        //Make the Scoreboard the last Bezier point
        pts.Add(rectTrans.anchorMax);

        //Set the value of the Floating Score
        int value = wyrd.letters.Count * combo;
        FloatingScore fs = Scoreboard.S.CreateFloatingScore(value, pts);

        fs.timeDuration = scoreTravelTime;
        fs.timeStart = Time.time + combo * scoreComboDelay;
        fs.fontSizes = scoreFontSize;

        //Double the InOut Easing effect
        fs.easingCurve = Easing.InOut + Easing.InOut;

        //Make the text of the FloatingScore something like "3 x 2"
        string txt = wyrd.letters.Count.ToString();
        if (combo > 1)
        {
            txt += " x " + combo;
        }
        fs.GetComponent<Text>().text = txt;
    }
}
