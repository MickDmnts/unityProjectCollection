using UnityEngine;

public class DW_Loops : MonoBehaviour 
{
    [Range(1,9)]
    public int repeatTime; //Input from Inspector

    private int _j; //Timer

    void Start() //Executes once
    {
        _j = repeatTime + 1; //set _j to the number of repeats and add 1 to avoid 0
        repeatTime = 1; //Set number of repeats to 1 and use it as a breakpoint
        Repeater(); //Call Repeater()
    }

    void Repeater()
    {
        do //Executes while breakpoint smaller than Timer
        {
            print("Loop #" + repeatTime);
            repeatTime++; //Increment by 1
        } while (repeatTime < _j);
    }
}
