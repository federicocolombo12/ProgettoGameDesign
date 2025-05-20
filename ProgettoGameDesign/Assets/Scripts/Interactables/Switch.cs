using System;
using UnityEngine;

public class Switch : Interactable
{
    [SerializeField] private GameObject switchObject;
    private SpriteRenderer sr;

    public override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
    }

    // The object that will be switched
    public override void Interact(GameObject interactor)
    {
        if (switchObject.GetComponent<IInteractable>() != null)
        {
            switchObject.GetComponent<IInteractable>().Interact(gameObject);
        }
        else
        {
            Debug.LogWarning("SwitchableObject component not found on the switch object.");
        }
    }
    
}

   

