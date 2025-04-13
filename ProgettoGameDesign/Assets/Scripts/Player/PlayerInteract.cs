using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    [SerializeField] float interactDistance = 1f;
    [SerializeField] LayerMask interactableLayer;
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
        // Avanza il timer, ma non oltre il cooldown
        if (interactionTimer < interactionCooldown)
        {
            interactionTimer += Time.deltaTime;
        }

        // Calcolo direzione e area di rilevamento
        Vector2 direction = transform.right.normalized;
        Vector2 boxCenter = (Vector2)transform.position + (direction * interactDistance / 2);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, interactableLayer);

        if (interacted && interactionTimer >= interactionCooldown)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider != coll)
                {
                    IInteractable interactable = collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact(gameObject);
                        interactionTimer = 0f; // reset dopo la prima interazione valida
                        break; // se vuoi interagire con un solo oggetto per frame
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
