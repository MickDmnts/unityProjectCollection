using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrailDestruction : MonoBehaviour
{
    [Header("Trail destruction timer - Set in inspector")]
    public float timeUntilDestruction = 1f;

    private void Start()
    {
        SetTimerAndDestroy();
    }

    void SetTimerAndDestroy()
    {
        Destroy(this.gameObject, timeUntilDestruction);
    }
}
