using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FE_Loops : MonoBehaviour {

    public Text inputText;
    public AudioSource clickSound;

    string _str = "Hello";

    // Use this for initialization
    void Start()
    {
        inputText.text = "";
        StartCoroutine(Repeater());
    }

    IEnumerator Repeater()
    {
        foreach (char chr in _str)
        {
            clickSound.Play();
            inputText.text = inputText.text + chr;
            yield return new WaitForSeconds(0.3f);
            clickSound.Stop();
        }
    }

}
