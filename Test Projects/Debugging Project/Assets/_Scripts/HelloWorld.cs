using UnityEngine;

public class HelloWorld : MonoBehaviour //MAIN SCRIPT
{
    public GameObject cubeToSpawn; 

    void Start() {
        //Instantiate(cubeToSpawn);
    }

    void Update() {
        Instantiate(cubeToSpawn);
    }

    private void OnApplicationQuit()
    {
        PrintOnExit(true);
    }

    void PrintOnExit(bool exiting = false)
    {
        if (exiting)
        {
            print("You just exited play mode!");
        }
    }
}