
using UnityEngine;

public interface IInteractable
{
    public void Detected(GameObject interactor);
    public void Interact(GameObject interactor);
}
