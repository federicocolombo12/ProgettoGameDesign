using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class Patrol : Action
{
    [Header("Movement")]
    public SharedFloat speed;
    public float patrolRange = 3f;

    [Header("Checks")]
    public float groundCheckDistance = 1f;
    public float wallCheckDistance = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Enemy enemy;
    private Vector2 startPos;
    private Vector2 target;
    private bool reachedTarget;

    public override void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        startPos = transform.position;
        reachedTarget = false;

        target = GenerateValidTarget();
    }

    public override TaskStatus OnUpdate()
    {
        if (reachedTarget)
            return TaskStatus.Success;

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        direction.y = 0;

        // Apply movement
        rb.linearVelocity = direction * speed.Value;

        // Flip enemy to face direction
        if (direction.x > 0)
            enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        else if (direction.x < 0)
            enemy.transform.localScale = new Vector3(-Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);

        // Check if close enough to the target
        if (Vector2.Distance(transform.position, target) < 0.3f)
        {
            rb.linearVelocity = Vector2.zero;
            reachedTarget = true;
            return TaskStatus.Success;
        }

        // Optional: stop early if walking into wall or ledge
        if (IsWallAhead() || IsLedgeAhead())
        {
            rb.linearVelocity = Vector2.zero;
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }

    private Vector2 GenerateValidTarget()
    {
        int attempts = 10;
        for (int i = 0; i < attempts; i++)
        {
            float offset = Random.Range(-patrolRange, patrolRange);
            Vector2 potential = new Vector2(startPos.x + offset, startPos.y);

            if (!Physics2D.Raycast(potential, Vector2.down, groundCheckDistance, groundLayer))
                continue;

            if (!Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(offset), Mathf.Abs(offset), groundLayer))
                return potential;
        }

        return startPos; // fallback
    }

    private bool IsWallAhead()
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        return Physics2D.Raycast(transform.position, dir, wallCheckDistance, groundLayer);
    }

    private bool IsLedgeAhead()
    {
        Vector2 offset = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 checkPos = (Vector2)transform.position + offset;
        return !Physics2D.Raycast(checkPos, Vector2.down, groundCheckDistance, groundLayer);
    }
}
