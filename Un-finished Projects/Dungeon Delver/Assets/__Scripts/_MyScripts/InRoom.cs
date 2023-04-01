using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoom : MonoBehaviour
{
    static public float ROOM_W = 16;
    static public float ROOM_H = 11;
    static public float WALL_THICKNESS = 2;

    [Header("Set in Inspector")]
    public bool keepInRoom = true;
    public float gridMult = 1;


    private void LateUpdate()
    {
        if (keepInRoom)
        {
            Vector2 rPos = roomPos;
            rPos.x = Mathf.Clamp(rPos.x, WALL_THICKNESS, ROOM_W - 1 - WALL_THICKNESS);
            rPos.y = Mathf.Clamp(rPos.y, WALL_THICKNESS, ROOM_H - 1 - WALL_THICKNESS);
            roomPos = rPos;
        }
    }

    public Vector2 roomPos
    {
        get
        {
            Vector2 tempPos = transform.position;
            tempPos.x %= ROOM_W;
            tempPos.y %= ROOM_H;
            return tempPos;
        }
        set
        {
            Vector2 roomNumber = roomNum;
            roomNumber.x *= ROOM_W;
            roomNumber.y *= ROOM_H;
            roomNumber += value;
            transform.position = roomNumber;
        }
    }

    public Vector2 roomNum
    {
        get
        {
            Vector2 tempPos = transform.position;
            tempPos.x = Mathf.Floor(tempPos.x/ROOM_W);
            tempPos.y = Mathf.Floor(tempPos.y / ROOM_H);
            return tempPos;
        }
        set
        {
            Vector2 rPos = roomPos;
            Vector2 rm = value;
            rm.x *= ROOM_W;
            rm.y *= ROOM_H;
            transform.position = rm + rPos;
        }
    }
}
