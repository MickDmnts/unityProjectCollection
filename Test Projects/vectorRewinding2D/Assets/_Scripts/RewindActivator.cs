using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindActivator : MonoBehaviour
{
    [SerializeField] VectorRecorder vectorRecorderRef;

    private void Start()
    {
        vectorRecorderRef = GetComponent<VectorRecorder>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            VectorRewinder.S.StartRewind(gameObject, vectorRecorderRef.GetPastCordsList());
    }
}
