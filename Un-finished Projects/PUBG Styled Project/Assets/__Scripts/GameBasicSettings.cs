using UnityEngine;

public class GameBasicSettings : MonoBehaviour
{
    public static GameBasicSettings S;

    public GameObject pickUpText;

    private void Awake()
    {
        S = this;
        pickUpText.SetActive(false);
    }

    public void Toggler(bool state) //PickUpText state toggle On-Off
    {
        if (state == true)
        {
            pickUpText.SetActive(true);
        }
        else
        {
            pickUpText.SetActive(false);
        }
    }
}
