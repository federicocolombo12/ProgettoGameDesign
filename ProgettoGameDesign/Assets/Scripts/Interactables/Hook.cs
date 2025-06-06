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
        if (sr.color == Color.red)
        {
            sr.color = Color.white;
        }
        lineRenderer.enabled = isVisible; // Disable the line renderer when not in use
    }
    public override void Detected(GameObject interactor)
    {
        base.Detected(interactor);
        //trace a line to the interactor
        if (interactor.GetComponent<PlayerHook>() != null)
        {
            
            lineRenderer.SetPosition(0, transform.position); // Set the start position of the line
            // Calculate the direction from the hook to the interactor
            Vector3 direction = (interactor.transform.position - transform.position).normalized;
            // Set the end position with an offset from the interactor along the direction
            Vector3 endPosition = interactor.transform.position - direction * hookOffset;
            lineRenderer.SetPosition(1, endPosition);

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
