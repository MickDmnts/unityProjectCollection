using UnityEngine;

public class SwordPickUp : MonoBehaviour
{
    static public SwordPickUp S;
    //---------------------------------

    public bool displayText;

    private void Awake()
    {
        S = this;
        displayText = false;
    }

    private void OnTriggerEnter(Collider other) //Enter
    {
        Debug.Log("Enter");
    }

    private void OnTriggerStay(Collider other) //Stay
    {
        if (other.tag == "Player")
        {
            if (displayText)
            {
                GameBasicSettings.S.Toggler(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) //Exit
    {
        Debug.Log("Exit");
    }
}
