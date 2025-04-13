using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerHook : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float hookSpeed = 10f;
    [SerializeField] float stopDistance = 0.1f;
    private PlayerStateList pState;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    
    }
    public void Hook(Vector2 hookPosition)
    {
        StartCoroutine(MoveToHook(hookPosition));
    }

    private IEnumerator MoveToHook(Vector2 target)
    {
        pState.hooked = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        while (Vector2.Distance(rb.position, target) > stopDistance)
        {
            Vector2 direction = (target - rb.position).normalized;
            rb.MovePosition(rb.position + direction * hookSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        pState.hooked = false;
    }
}
