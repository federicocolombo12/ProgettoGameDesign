using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    [SerializeField] float interactDistance = 1f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float interactionCooldown = 0.5f;
    private float interactionTimer;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    public void PlayerCheckForInteractables(bool interacted)
    {
        if (interactionTimer < interactionCooldown)
        {
            interactionTimer += Time.deltaTime;
        }

        Vector2 direction = transform.right.normalized;
        Vector2 boxCenter = (Vector2)transform.position + (direction * interactDistance / 2);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, interactableLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider != coll)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    

                    // Linecast per controllare se c'è un muro in mezzo
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, collider.transform.position, wallLayer);
                
                    if (hit.collider != null)
                    {
                        // C'è un muro in mezzo: ignora questo oggetto
                        continue;
                    }
                    interactable.Detected(gameObject);

                    if (interacted && interactionTimer >= interactionCooldown)
                    {
                        interactable.Interact(gameObject);
                        interactionTimer = 0f;
                        break; // Interagisce con il primo oggetto valido
                    }
                }
            }
        }
    

        
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 direction = transform.right.normalized;
        Vector2 boxCenter = (Vector2)transform.position + direction * interactDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

}
