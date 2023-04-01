using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendering : MonoBehaviour
{
    static public LineRendering S;

    [Header("LR Main Components")]
    public LineRenderer lr;
    public Transform playerTran;

    private void Start()
    {
        lr.enabled = true;
        S = this;
    }

    private void Update()
    {
        lr.SetPosition(0, playerTran.transform.position); //start pos

        Vector3 mouse2D = Input.mousePosition;
        mouse2D.z = -Camera.main.transform.position.z;
        Vector3 mouse3D = Camera.main.ScreenToWorldPoint(mouse2D);

        lr.SetPosition(1, mouse3D);//end pos
    }

    public void Clear(int state)
    {
        switch (state)
        {
            case 0:
                lr.startWidth = 0.0f;
                lr.endWidth = 0.0f; //non-visible
                break;
            case 1:
                lr.startWidth = 0.01f;
                lr.endWidth = 0.01f;
                break;
        }
    }
}
