using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListEx : MonoBehaviour
{
    public List<string> sList;

    void Start() {
        sList = new List<string>();

        sList.Add("Experience");
        sList.Add("is");
        sList.Add("what");
        sList.Add("you");
        sList.Add("get");
        sList.Add(", when");
        sList.Add("you");
        sList.Add("don't");
        sList.Add("get");
        sList.Add("what");
        sList.Add("you");
        sList.Add("wanted.");

        print("sList Count = " + sList.Count);
        print("The 0th element is: " + sList[0]);
        print("The 1th element is: " + sList[1]);
        print("The 3th element is: " + sList[3]);
        print("The 8th element is: " + sList[8]);

        string str = "";
        foreach (string sTemp in sList)
        {
            str += sTemp + " ";
        }
        print(str);
    }
}
