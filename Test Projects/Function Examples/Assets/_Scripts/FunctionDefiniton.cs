using UnityEngine;

public class FunctionDefiniton : MonoBehaviour
{
    public int updatesCalled = 0;

    void Update() {
        updatesCalled++;
        PrintUpdates();
    }

    void PrintUpdates() {
        string outputMsg = "Update #" + updatesCalled;
        print(outputMsg);
    }
}
