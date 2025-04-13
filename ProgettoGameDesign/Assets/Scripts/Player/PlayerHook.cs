using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerHook : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [SerializeField] float stopDistance = 0.1f;
    [SerializeField] float initialHookSpeed = 0.1f; // Velocità iniziale del rampino
    [SerializeField] float hookAcceleration = 0.1f; // Accelerazione del rampino
    [SerializeField] float hookMaxSpeed = 5f; // Velocità massima del rampino
    [SerializeField] float waitTime = 0.1f; // Tempo di attesa prima di iniziare il movimento del rampino
    
    private PlayerStateList pState;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
       
    
    }
    public void Hook(Vector2 hookPosition)
    {
        if (Player.Instance.playerTransformation.abilityType==PlayerTransformation.AbilityType.GrapplingHook)
        {
            StartCoroutine(MoveToHook(hookPosition));
        }
        
    }

    private IEnumerator MoveToHook(Vector2 target)
    {
        yield return new WaitForSeconds(waitTime); // Attendi un breve momento prima di iniziare il movimento
        pState.hooked = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        Vector2 direction = (target - rb.position).normalized;
        
        float currentSpeed = initialHookSpeed; // Velocità iniziale diversa da zero
        float acceleration = hookAcceleration; // Accelerazione costante
        float maxSpeed = hookMaxSpeed; // Velocità massima del rampino

        while (Vector2.Distance(rb.position, target) > stopDistance)
        {
            // Aggiorna la velocità corrente con l'accelerazione, senza superare la velocità massima
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);

            // Muove il personaggio nella direzione del target con la velocità corrente
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        // Assicura che il personaggio raggiunga esattamente il target
        rb.MovePosition(target);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        pState.hooked = false;
    }



}
