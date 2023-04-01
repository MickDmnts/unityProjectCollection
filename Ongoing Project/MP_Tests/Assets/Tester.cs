using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System.IO;

public class Tester : MonoBehaviour
{
    MSGTest test = new MSGTest();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            test.ExecuteTest();
        }
    }
}

public class MSGTest
{
    public void ExecuteTest()
    {
        User temp = new User("2", "Michael");

        byte[] ser = MessagePackSerializer.Serialize(temp);

        File.WriteAllBytes(Path.Combine(Application.dataPath, "temp.txt"), ser);
    }
}