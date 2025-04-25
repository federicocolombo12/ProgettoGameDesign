using UnityEngine;

public class PlayerBreakableRock : MonoBehaviour
{
    public float chargeSpeed = 10f;
    public float chargeDuration = 0.3f;
    private bool isCharging = false;
    private float chargeTimer = 0f;
    private Vector2 chargeDirection;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input per avviare la carica (puoi cambiarlo con un altro trigger)
        if (Input.GetKeyDown(KeyCode.Space) && !isCharging)
        {
            StartCharge();
        }

        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            rb.linearVelocity = chargeDirection * chargeSpeed;

            if (chargeTimer <= 0f)
            {
                StopCharge();
            }
        }
    }

    void StartCharge()
    {
        isCharging = true;
        chargeTimer = chargeDuration;

        // Calcola la direzione in cui caricare (in questo caso verso dove guarda il player)
        chargeDirection = transform.right.normalized; // oppure basato su input o un target
    }

    void StopCharge()
    {
        isCharging = false;
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsCharging()
    {
        return isCharging;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCharging)
        {
            BreakableRock rock = other.GetComponent<BreakableRock>();
            if (rock != null)
            {
                // Interagisce con la roccia per distruggerla
                rock.Interact(gameObject);
            }
        }
    }
}
