using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Audio
{
    class AudioStatics
    {
        private static int _audioID = 0;
        public static int AudioID { get { return _audioID++; } }
    }

    public int audioID;

    [Header("Set in inspector")]
    public string audioName;
    public AudioClip audioClip;
    [Range(0, 1f)] public float volume;

    [Header("Set Dynamically")]
    public AudioSource audioSource;

    public void SetAudioID()
    {
        audioID = AudioStatics.AudioID;
    }

    public override string ToString()
    {
        return $"{audioName} with ID {audioID}\n" +
            $"contains audio file named: {audioClip}";
    }
}

public class SoundManager : MonoBehaviour
{
    //PseudoSingleton
    public static SoundManager S;

    public List<Audio> audioList;

    private void Awake()
    {
        //Avoid duplication
        if (S != null)
        {
            Destroy(this);
        }
        else
        {
            S = this;
        }

        SetAudioIDs();
    }

    void SetAudioIDs()
    {
        foreach (Audio audioObj in audioList)
        {
            audioObj.SetAudioID();
            print($"ID at {audioObj}");
        }
    }
}
