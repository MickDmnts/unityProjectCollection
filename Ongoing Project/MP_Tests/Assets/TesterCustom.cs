using System.IO;
using MessagePack;
using UnityEngine;

public class TesterCustom
{
    public TesterCustom() { }

    public void ExecuteTest()
    {
        User user = new User("2", "Michael");
        byte[] msg = MessagePackSerializer.Serialize(user);

        File.WriteAllBytes(Path.Combine(Application.dataPath, "temp.txt"), msg);
    }
}
