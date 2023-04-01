using UnityEngine;

[CreateAssetMenu(fileName = "AbilityManager", menuName = "ScriptableObjects/AbilityManager", order = 0)]
public class AbilityManager : ScriptableObject
{
    //The ability selected from the main menu.
    [SerializeField] Ability equipedAbility;

    /// <summary>
    /// Call to set the currently equiped ability to the given parameter.
    /// </summary>
    public void SetAbility(Ability abilityType)
    {
        equipedAbility = abilityType;
    }

    /// <summary>
    /// Call to get the currently equiped ability.
    /// </summary>
    public Ability GetEquipedAbility()
    {
        if (equipedAbility == null)
        {
            Debug.Log("Equiped ability is empty");
            return null;
        }

        return equipedAbility;
    }
}