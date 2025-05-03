using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    public SharedFloat patrolRange = 3f;
    public SharedFloat speed = 2f;
    public LayerMask wallLayerMask;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 target;
    private bool movingRight = true;

    public override void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        SetNextTarget();
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        // Movimento
        rb.linearVelocity = dir * speed.Value;

        // Flip sprite
        if (dir.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Raggiunto il target?
        if (Vector2.Distance(transform.position, target) < 0.3f)
        {
            movingRight = !movingRight;
            SetNextTarget();
        }

        return TaskStatus.Running;
    }

    private void SetNextTarget()
    {
        Vector2 offset = movingRight ? Vector2.right : Vector2.left;
        Vector2 potentialTarget = startPosition + offset * patrolRange.Value;

        // Controllo ostacoli
        RaycastHit2D hit = Physics2D.Linecast(transform.position, potentialTarget, wallLayerMask);
        if (hit.collider != null)
        {
            movingRight = !movingRight;
            offset = -offset;
            potentialTarget = startPosition + offset * patrolRange.Value;
        }

        target = potentialTarget;
    }
}
