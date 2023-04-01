using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;

    GameController gameController;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            gameController.interactionDescriptionInRoom.Add(currentRoom.exits[i].exitDescription);
        }
    }
}
