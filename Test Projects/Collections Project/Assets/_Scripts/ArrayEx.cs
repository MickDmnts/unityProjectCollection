using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayEx : MonoBehaviour
{
    public string[] sArray;

    void Start() {
        sArray = new string[10];

        sArray[0] = "These";
        sArray[1] = "are";
        sArray[2] = "some";
        sArray[3] = "words";

        print("The length of the array is: " + sArray.Length);

        string str = "";
        foreach (string sTemp in sArray)
        {
            str += "|" + sTemp;
            if (sTemp == null)
            {
                break;
            }
        }
        print(str);
    }
}
