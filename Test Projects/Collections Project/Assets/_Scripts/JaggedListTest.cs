using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaggedListTest : MonoBehaviour
{
    public List<List<string>> jaggedList;

    void Start() {
        jaggedList = new List<List<string>>
        {
            new List<string>(), //Adds two list strings to the jagged list
            new List<string>()
        };

        jaggedList[0].Add("Hello"); //Adds two strings to the jaggedList[0]
        jaggedList[0].Add("World");

        jaggedList.Add(new List<string>(new string[] { "Complex", "Initialization" })); //Complex Init

        string str = "";
        foreach (List<string> sL in jaggedList)
        {
            foreach (string sTemp in sL)
            {
                if (sTemp != null)
                {
                    str += " | " + sTemp;
                }
                else
                {
                    str += " | ";
                }
            }
            str += " | \n";
        }
        print(str);
    }
}
