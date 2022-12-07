using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    endLevel,
}

public class GameController : MonoBehaviour
{
    //Variable section------------------------------------------------------------------
    static private GameController S;
    private const int timeBack = 20;

    [Header("Set in Inspector")]
    public Text uitLevel; //the level text
    public Text uitShots; //the shots text
    public Text uitButton; //text inside view button
    public Text uitRestart; //text inside restart button
    public Text uitReYes; //the text in yes btn
    public Text uitReNo; //the text in no btn
    public GameObject blurEffect; //the restart blurring
    public GameObject panel; //the restart panel
    public Button restartButton; //the restart button
    public Vector3 castleSpawn; //the castle spawn pos
    public GameObject[] castleArray; //all the castles - aka levels


    [Header("Set Dynamically (DO NOT MODIFY)")]
    public int level; //current level
    public int levelMax; //max # of lvls
    public int shotsTaken; //shots taken
    public GameObject castle; //the current castle
    public GameMode mode = GameMode.idle; //current mode
    public string showing = "Show Slingshot"; //what is the camera showing
    
    //Variable section------------------------------------------------------------------

    private void Start()
    {
        S = this;
        level = 0;
        levelMax = castleArray.Length;
        panel.SetActive(false);
        blurEffect.SetActive(false);
        StartLevel();
    }

    //StartLevel Section----------------------------------------------------------------
    void StartLevel()
    {
        //get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destroy every projectile that exists
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        //Instantiate the new castle
        castle = Instantiate(castleArray[level]);
        castle.transform.position = castleSpawn;
        shotsTaken = 0;

        //Reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //reset the goal
        GoalReached.goalReached = false;

        UpdateGUI(); //Update the GUI

        mode = GameMode.playing;
        StartCoroutine(StartView());
    }

    IEnumerator StartView()
    {
        yield return new WaitForSeconds(2f);
        SwitchView("Show Slingshot");
    }
    //StartLevel Section----------------------------------------------------------------


    //Restart section------------------------------------------------------------------
    public void RestartButton() //restart the level
    {
        blurEffect.SetActive(true);
        restartButton.enabled = false;
        panel.SetActive(true);
    }

    public void yesRestart() //if yes pressed -- panel
    {
        panel.SetActive(false);
        StartLevel();
        StartCoroutine(stopRestartSpam());
    }

    public void noRestart() //if no pressed -- panel
    {
        panel.SetActive(false);
        StartCoroutine(stopRestartSpam());
    }

    IEnumerator stopRestartSpam()
    {
        blurEffect.SetActive(false);
        yield return new WaitForSeconds(3f);
        restartButton.enabled = true;
    }
    //Restart section--------------------------------------------------------------------

    //GUI Section------------------------------------------------------------------------
    void UpdateGUI()
    {
        //Show the data in the GUI texts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots: " + shotsTaken;
        uitRestart.text = "Restart";
        uitReYes.text = "Yes";
        uitReNo.text = "No";
    }
    //GUI Section------------------------------------------------------------------------

    //Level check section----------------------------------------------------------------
    private void Update()
    {
        UpdateGUI();
        //Check for level end
        if ((mode == GameMode.playing) && GoalReached.goalReached == true)
        {
            //change mode to stop checking for level end
            mode = GameMode.endLevel;

            //Zoom out
            SwitchView("Show Both");

            //start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() //proceed to next level
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }


    public void SwitchView(string view = "") //Switch the camera view 
    {
        if (view == "")
        {
            view = uitButton.text;
        }

        showing = view;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    //Level check section----------------------------------------------------------------

    //Misc section-------------------------------------------------------------------
    public static void ShotFired() //public static used from the slingshot
    {
        S.shotsTaken++;
    }
}
