using UnityEngine;

public class Hook : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out PlayerHook playerHook))
        {
            playerHook.Hook(transform.position);
        }
    }
}
