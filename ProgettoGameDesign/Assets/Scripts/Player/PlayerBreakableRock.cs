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
        chargeDirection = transform.right.normalized; 
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
              
                rock.Interact(gameObject);
            }
        }
    }
}
