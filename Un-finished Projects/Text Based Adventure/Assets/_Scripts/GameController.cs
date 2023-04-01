using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Set in inspector")]
    public Text displayTextArea;
    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public List<string> interactionDescriptionInRoom = new List<string>();

    List<string> actionLog = new List<string>();

    private void Awake()
    {
        roomNavigation = GetComponent<RoomNavigation>();
    }

    public void Start()
    {
        DisplayRoomDescription();
        DisplayLoggedText();
    }

    void DisplayRoomDescription()
    {
        UnpackRoom();
        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionInRoom.ToArray());

        string compinedText = roomNavigation.currentRoom.roomDescription + "\n" + joinedInteractionDescriptions;
        LogStringWithReturn(compinedText);
    }
   
    void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

    void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());
        displayTextArea.text = logAsText;
    }
}
