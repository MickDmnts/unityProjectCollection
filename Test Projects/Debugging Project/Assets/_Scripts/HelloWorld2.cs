using UnityEngine;

public class HelloWorld2 : MonoBehaviour
{
    public GameObject cubeToSpawn;

    void Start() {
        //Instantiate(cubeToSpawn);
    }

    void Update() {
        Instantiate(cubeToSpawn);
        SpellItOut();
    }

    void SpellItOut()
    {
        string sA = "Hello World";
        string sB = "";

        for (int i = 0; i < sA.Length; i++)
        {
            sB += sA[i];
        }

        print(sB);
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