using UnityEngine;

public class Hook : Interactable
{
    SpriteRenderer sr;
    LineRenderer lineRenderer;
    [SerializeField] float hookOffset = 2f; // Offset for the hook position
    
    
    public override void Start()
    {

        base.Start();
        interactionEffect = GetComponentInChildren<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        lineRenderer.enabled = false; // Disable the line renderer initially

    }
    public override void Update()
    {
        // Optional: Add any update logic if needed
        base.Update();
       
        
    }
    public override void Detected(GameObject interactor)
    {
        base.Detected(interactor);
        //trace a line to the interactor
        
        
        

    }
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        if (interactor.GetComponent<PlayerHook>() != null)
        {

            interactor.GetComponent<PlayerHook>().Hook(transform.position);


        }

    }
    
    
}
