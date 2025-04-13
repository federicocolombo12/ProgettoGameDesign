using UnityEngine;

public class Hook : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerHook>() != null)
        {
            
            interactor.GetComponent<PlayerHook>().Hook(transform.position);
            
        }
        
    }
}
