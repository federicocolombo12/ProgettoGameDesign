using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Hook(Vector2 hookPosition)
    {
        Vector2 direction = (hookPosition - rb.position).normalized;
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }
}
