using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text Adventure Game/Room")]
public class Room : ScriptableObject
{
    [Header("Set in inspector")]
    [TextArea]
    public string roomDescription;
    public string roomName;
    public Exit[] exits;
}
