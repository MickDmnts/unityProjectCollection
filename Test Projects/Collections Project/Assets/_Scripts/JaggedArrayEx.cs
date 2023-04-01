using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaggedArrayEx : MonoBehaviour
{
    public string[][] jArray;

    void Start() {
        jArray = new string[4][];

        jArray[0] = new string[4];
        jArray[0][0] = "A";
        jArray[0][1] = "B";
        jArray[0][2] = "C";
        jArray[0][3] = "D";

        jArray[1] = new string[] { "E", "F", "G" };
        jArray[2] = new string[] { "H", "I" };

        jArray[3] = new string[4];
        jArray[3][0] = "J";
        jArray[3][3] = "K";

        print("The length of the jArray is: " + jArray.Length);

        //--------------------------------------------------------

        print("The lentgth of the jArray[1] is: " + jArray[1].Length);

        string str = "";
        for (int i = 0; i < jArray.Length; i++)
        {
            for (int j = 0; j < jArray[i].Length; j++)
            {
                if (jArray[i][j] != null)
                {
                    str += " | " + jArray[i][j]; 
                }
                else
                {
                    str += " |  ";
                }
            }
            str += " | \n";
        }
        print(str);
    }
}
