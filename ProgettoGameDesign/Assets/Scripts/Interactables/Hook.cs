using UnityEngine;

public class Hook : Interactable
{
    SpriteRenderer sr;
    public override void Start()
    {
        
        base.Start();
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
    public override void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerHook>() != null)
        {
            
            interactor.GetComponent<PlayerHook>().Hook(transform.position);
            
            sr.color = Color.green;
        }
        
    }
    
    
}
