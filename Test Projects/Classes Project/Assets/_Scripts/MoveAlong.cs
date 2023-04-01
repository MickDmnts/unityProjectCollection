using UnityEngine;
using UnityEditor;
using System.Collections;

public class MoveAlong : MonoBehaviour
{
    private CountItHigher cih;

    private void Start()
    {
        cih = this.gameObject.GetComponent<CountItHigher>();
    }

    private void LateUpdate()
    {
        if (cih != null)
        {
            float tX = cih.currentNum / 10;
            Vector3 tempLoc = Pos;
            tempLoc.x = tX;
            Pos = tempLoc;
        }
        else
        {
            PrintOnExit(true);
        }
    }

    void PrintOnExit(bool exiting = false)
    {
        if (exiting)
        {
            print("No CountItHigher script attached to this object.");
            EditorApplication.isPlaying = false;
        }
    }

    public Vector3 Pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }
}
