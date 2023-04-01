using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActivator : MonoBehaviour
{
    [SerializeField] AbilityManager abilityManager;

    void Start()
    {
        SwitchOnAbility();
    }

    void SwitchOnAbility()
    {
        Ability tempAbility = abilityManager.GetEquipedAbility();

        if (tempAbility == null)
            return;

        switch (tempAbility.abilityName)
        {
            case AbilityType.SlowDownTime:
                Debug.Log($"{tempAbility.abilityName}" +
                    $"{tempAbility.abilityDescription}");
                break;
            case AbilityType.StopTime:
                Debug.Log($"{tempAbility.abilityName}" +
                    $"{tempAbility.abilityDescription}");
                break;
            case AbilityType.TimeRewind:
                Debug.Log($"{tempAbility.abilityName}" +
                    $"{tempAbility.abilityDescription}");
                break;
            default:
                break;
        }
    }
}
