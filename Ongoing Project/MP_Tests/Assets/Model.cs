using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
public class User
{
    [Key(0)]
    public string _id;
    [Key(1)]
    public string _name;

    public User(string setId, string setName)
    {
        _id = setId;
        _name = setName;
    }

    /// <summary>
    /// Returns the user's id.
    /// </summary>
    [IgnoreMember]
    public string UserID
    {
        get { return _id; }
    }

    /// <summary>
    /// Returns the user's name.
    /// </summary>
    [IgnoreMember]
    public string Name
    {
        get { return _name; }
    }
}