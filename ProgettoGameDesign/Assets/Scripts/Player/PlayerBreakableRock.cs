using System.Collections;
using UnityEngine;

public class PlayerBreakableRock : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStateList pState;

    [SerializeField] float chargeSpeed = 0.1f;              // Velocit� iniziale della carica
    [SerializeField] float chargeAcceleration = 0.1f;       // Accelerazione della carica
    [SerializeField] float chargeMaxSpeed = 8f;             // Velocit� massima
    [SerializeField] float chargeDuration = 0.5f;           // Durata totale della carica
    [SerializeField] float deceleration = 5f;               // Decelerazione finale (inerzia)
    [SerializeField] float waitTime = 0.1f;                 // Attesa prima della carica

    private bool isCharging = false;
    public bool IsCharging() => isCharging;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    }

    public void BreakCharge(Vector2 target)
    {
        if (Player.Instance.playerTransformation.abilityType == PlayerTransformation.AbilityType.ChargingRockBreaker)
        {
            StartCoroutine(ChargeToRock(target));
        }
    }

    private IEnumerator ChargeToRock(Vector2 target)
    {
        isCharging = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(waitTime);

        Vector2 direction = (target - rb.position).normalized;

        float currentSpeed = chargeSpeed;
        float timer = 0f;

        // Fase 1: accelerazione e carica verso il target
        while (timer < chargeDuration)
        {
            currentSpeed = Mathf.Min(currentSpeed + chargeAcceleration * Time.fixedDeltaTime, chargeMaxSpeed);
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Fase 2: decelerazione graduale
        while (currentSpeed > 0)
        {
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.fixedDeltaTime, 0);
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        isCharging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCharging)
        {
            BreakableRock rock = other.GetComponent<BreakableRock>();
            if (rock != null)
            {
                rock.Interact(gameObject);
            }
        }
    }
}
