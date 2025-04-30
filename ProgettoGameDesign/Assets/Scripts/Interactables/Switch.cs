using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject switchObject; // The object that will be switched
    public void Interact(GameObject interactor)
    {
        if (switchObject.GetComponent<SwitchableObject>() != null)
        {
            switchObject.GetComponent<SwitchableObject>().Interact();
        }
        else
        {
            Debug.LogWarning("SwitchableObject component not found on the switch object.");
        }
    }
    public void Detected(GameObject interactor)
    {
        // Implement the logic for when the interactor is detected
        Debug.Log("Switch detected by: " + interactor.name);
    }
}

   

