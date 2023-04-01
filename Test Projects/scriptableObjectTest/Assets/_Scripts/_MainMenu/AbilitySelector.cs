using UnityEngine;

/// <summary>
/// This enum keeps track of all the created abilities,
/// P.S. The enum numbers MUST be equal to the Ability[] indexing
/// </summary>
public enum AbilityType
{
    SlowDownTime = 0,
    StopTime = 1,
    TimeRewind = 2,
}

public class AbilitySelector : MonoBehaviour
{
    [Header("Drag S.O. Reference")]
    [SerializeField] AbilityManager abilityManager;

    [Header("Select starting ability")]
    [SerializeField] AbilityType abilityType;

    [Header("Populate with created abilities")]
    [SerializeField] Ability[] abilities;

    private void Start()
    {
        abilityManager.SetAbility(abilities[(int)abilityType]);
    }
}
