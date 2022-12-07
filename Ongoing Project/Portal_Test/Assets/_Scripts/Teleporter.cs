using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Set in inspector")]
    [Range(0f, 5f)]
    [SerializeField] private float teleportSpeed;
    [Range(0f, 1f)]
    [SerializeField] private float timerFillSpeed;

    [Header("Which TP is that?")]
    [SerializeField] private bool blueTP;
    [SerializeField] private bool orangeTP;

    [Header("Set dynamically")]
    [SerializeField] private TPStatics tp_Statics;
    [SerializeField] private Transform teleportTrans;
    [SerializeField] private float timer;

    void Start()
    {
        SetTPStatics();
        SetTPLocation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            TPStatics.timer += timerFillSpeed * Time.deltaTime;
            tp_Statics.tp_slider.value = TPStatics.timer;

            if (TPStatics.timer >= teleportSpeed)
            {
                TPStatics.timer = 0f;
                tp_Statics.tp_slider.value = TPStatics.timer;

                other.gameObject.SetActive(false);
                other.transform.position = teleportTrans.position;
                other.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TPStatics.timer = 0f;
        tp_Statics.tp_slider.value = TPStatics.timer;
    }

    void SetTPStatics()
    {
        tp_Statics = GameObject.FindGameObjectWithTag("TP_Manager").GetComponent<TPStatics>();

        TPStatics.timer = 0f;
        tp_Statics.tp_slider.value = TPStatics.timer; // Default state
    }

    void SetTPLocation()
    {
        if (orangeTP)
        {
            teleportTrans = tp_Statics.tp_points[0];
        }
        else
        {
            teleportTrans = tp_Statics.tp_points[1];
        }
    }
}
