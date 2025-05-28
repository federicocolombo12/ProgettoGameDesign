using System.Collections;
using UnityEngine;
using DG.Tweening;
public class PlayerHook : MonoBehaviour
{
    private Rigidbody2D rb;


    [SerializeField] float initialHookSpeed = 0.1f; // Velocità iniziale del rampino
    [SerializeField] float hookAcceleration = 0.1f; // Accelerazione del rampino
    [SerializeField] float hookMaxSpeed = 5f; // Velocità massima del rampino
    [SerializeField] float waitTime = 0.1f;
    [SerializeField] GameObject hookTrailPrefab; // Prefab del trail del rampino
    [SerializeField] float particleOffset = 0.5f; // Offset per la posizione del trail del rampino

    // Nuova variabile: decelerazione per simulare l'inerzia dopo il target
    [SerializeField] float hookDeceleration = 5f;

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
        // Attendi un breve momento prima di iniziare il movimento
        pState.hooked = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);

        Player.Instance.animator.SetBool("SpAbility", true);

        

        // Calcola la direzione dal punto di partenza al target
        Vector2 direction = (target - rb.position).normalized;
        
        //calcola la rotazione in base alla direzione
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //calcola il quaternion per la rotazione
        
       
        GameObject hookTrail = Instantiate(hookTrailPrefab, rb.position + direction*particleOffset, Quaternion.identity);
        hookTrail.transform.DORotate(new Vector3(0, 0, angle), 0.1f);
        hookTrail.transform.DOMove(target, 1f).SetEase(Ease.Linear);
        float currentSpeed = initialHookSpeed;
        float acceleration = hookAcceleration;
        float maxSpeed = hookMaxSpeed;

        // FASE 1: Accelerazione (continua finché non si raggiunge o si supera il target)
        // Usiamo il prodotto scalare per capire se abbiamo già superato il target lungo la direzione
        while (Vector2.Dot(target - rb.position, direction) > 0)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        // FASE 2: Decelerazione (l'inerzia spinge oltre il target, poi il giocatore rallenta)
        // La decelerazione continuerà finché la velocità raggiunge zero
        while (currentSpeed > 0)
        {
            currentSpeed = Mathf.Max(currentSpeed - hookDeceleration * Time.fixedDeltaTime, 0);
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        // Alla fine azzeriamo la velocità ed eventualmente correggiamo la posizione
        Player.Instance.animator.SetBool("SpAbility", false);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        pState.hooked = false;
    }
}
