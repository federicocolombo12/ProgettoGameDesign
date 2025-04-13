using UnityEngine;

public class Hook : MonoBehaviour, IInteractable
{
    SpriteRenderer sr;
    public void Start()
    {
        
       
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }
    public void Update()
    {
        // Optional: Add any update logic if needed
        
        if (sr.color == Color.red)
        {
            sr.color = Color.white;
        }
    }
    public void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerHook>() != null)
        {
            
            interactor.GetComponent<PlayerHook>().Hook(transform.position);
            
            sr.color = Color.green;
        }
        
    }
    public void Detected(GameObject interactor)
    {
        
        if (interactor.GetComponent<PlayerHook>() != null)
        {
            sr.color = Color.red;
        }
        
    }
    
}
