using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryEx : MonoBehaviour
{
    public Dictionary<string, string> statesDict; //Won't show up in the inspector despite being public

    void Start() {
        statesDict = new Dictionary<string, string>();

        statesDict.Add("MD", "Maryland");
        statesDict.Add("TX", "Texas");
        statesDict.Add("PA", "Pensylvania");
        statesDict.Add("CA", "California");
        statesDict.Add("MI", "Michigan");

        print("There are " + statesDict.Count + " elements in statesDict.");

        foreach (KeyValuePair<string,string> kvp in statesDict)
        {
            print(kvp.Key + ": " + kvp.Value);
        }

        print("MI is " + statesDict["MI"]); //Bracket Access

        statesDict["BC"] = "British Columbia"; //Alternative to .Add()

        foreach (string k in statesDict.Keys)
        {
            print(k + " is " + statesDict[k]);
        }
    }
}
