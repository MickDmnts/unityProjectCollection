using UnityEngine;

///<summary>A primitive Player class.</summary>
public class Player : MonoBehaviour, IInteractable
{
    ///<summary>Called when a gameObject with a collider is inside a triggers' area constantly.</summary>
    private void OnTriggerStay(Collider other)
    {
        //Check and simultaneously get the component that is of type IInteractable in the trigger of the area.
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            //Call the interface method which then calls the retrieved objs ExternalInteraction.
            interactable.ExternalInteraction(gameObject.name);
        }
    }

    ///<summary>Prints a debug log representing the external interaction of the object.</summary>
    public void ExternalInteraction(string sourceName)
    {
        Debug.Log($"{sourceName} is in my vicinity.");
    }
}
