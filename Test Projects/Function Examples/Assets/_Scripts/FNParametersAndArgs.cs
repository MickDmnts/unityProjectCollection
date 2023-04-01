using UnityEngine;

public class FNParametersAndArgs : MonoBehaviour
{
    void Start() {
        Say("Hello World");
    }

    void Say(string sayThis) {
        print(sayThis);
    }
}
