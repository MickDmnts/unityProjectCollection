using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    GameObject sword;
    Dray dray;

    private void Start()
    {
        sword = transform.Find("Sword").gameObject;
        dray = transform.parent.GetComponent<Dray>();
        sword.SetActive(false);
    }

    void ToggleDraySword()
    {
        if (sword.activeSelf)
        {
            sword.SetActive(false);
        }
        else
        {
            sword.SetActive(true);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
        sword.SetActive(CheckDrayState());
    }

    bool CheckDrayState()
    {
        if (dray.mode == Dray.eMode.attack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
