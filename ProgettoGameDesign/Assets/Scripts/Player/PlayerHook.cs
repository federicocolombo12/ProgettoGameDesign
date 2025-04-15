using System.Collections;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] float stopDistance = 0.1f;
    [SerializeField] float initialHookSpeed = 0.1f; // Velocit� iniziale del rampino
    [SerializeField] float hookAcceleration = 0.1f; // Accelerazione del rampino
    [SerializeField] float hookMaxSpeed = 5f; // Velocit� massima del rampino
    [SerializeField] float waitTime = 0.1f; // Tempo di attesa prima di iniziare il movimento del rampino
    [SerializeField] float inertiaDuration = 0.5f; // Durata dell'inerzia
    [SerializeField] float inertiaForce = 5f; // Forza dell'inerzia

    private PlayerStateList pState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    }

    public void Hook(Vector2 hookPosition)
    {
        if (Player.Instance.playerTransformation.abilityType == PlayerTransformation.AbilityType.GrapplingHook)
        {
            StartCoroutine(MoveToHook(hookPosition));
        }
    }

    private IEnumerator MoveToHook(Vector2 target)
    {
        // Fase 1: Ritardo iniziale
        yield return new WaitForSeconds(waitTime); // Breve pausa prima dell'inizio del movimento

        pState.hooked = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        Vector2 direction = (target - rb.position).normalized;

        // Fase 2: Lancio del rampino - Scatto iniziale verso il bersaglio
        float launchSpeed = initialHookSpeed; // Velocità iniziale per il lancio
        float launchDuration = 0.1f; // Durata della fase di lancio
        float elapsedTime = 0f;

        while (elapsedTime < launchDuration)
        {
            rb.MovePosition(rb.position + direction * launchSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Fase 3: Traversata - Accelerazione verso il bersaglio
        float currentSpeed = launchSpeed;
        float acceleration = hookAcceleration;
        float maxSpeed = hookMaxSpeed;

        while (Vector2.Distance(rb.position, target) > stopDistance)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        // Assicura che il personaggio raggiunga esattamente il bersaglio
        rb.MovePosition(target);

        // Applica l'inerzia
        rb.linearVelocity = direction * inertiaForce;
        yield return new WaitForSeconds(inertiaDuration);

        // Ferma il movimento e ripristina la gravità
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        pState.hooked = false;
    }


}
