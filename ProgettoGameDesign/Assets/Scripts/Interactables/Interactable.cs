using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public virtual void Detected(GameObject interactor)
    {
        Debug.Log("Detected " + interactor.name + " on " + gameObject.name);
        // Optional: Add any detected logic if needed
    }

    public virtual void Interact(GameObject interactor)
    {
        Debug.Log("Interacting with " + interactor.name + " on " + gameObject.name);
        // Optional: Add any interact logic if needed
    }
    [SerializeField] protected CanvasGroup interactableUi;
    void Start()
    {
        interactableUi = GetComponentInChildren<CanvasGroup>();
    }
}
