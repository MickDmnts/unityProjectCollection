using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Loops : MonoBehaviour
{
    public string wordToRepeat;

    void Start()
    {
        for (int i = 1; i < wordToRepeat.Length +1; i++) //Repeats 1 time for each letter
        {
            print("Repeat #" + i + ": " + wordToRepeat);
        }
    }
}
