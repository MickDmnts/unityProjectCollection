using UnityEngine;

public class returningVoid : MonoBehaviour
{
    private string hisName = "Phill";

    void Start() {
        PrintIt(hisName);
        print("I'm out");
    }

    void PrintIt(string theName) {
        if (theName == hisName)
        {
            return; //breaks out of the function
        }
    }
}
