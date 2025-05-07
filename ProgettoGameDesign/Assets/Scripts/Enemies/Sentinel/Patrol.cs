using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    public SharedFloat speed;
    public float patrolRange = 3f;
    public LayerMask wallLayerMask;

    private Rigidbody2D rb;
    private Enemy enemy;
    private Vector2 startPos;
    private Vector2 target;

    public override void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        startPos = transform.position;
        target = GenerateTargetInRange();
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        dir.y = 0;

        rb.linearVelocity = dir * speed.Value;

        // Flip del nemico
        if (dir.x > 0)
            enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        else if (dir.x < 0)
            enemy.transform.localScale = new Vector3(-Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);

        if (Vector2.Distance(transform.position, target) < 0.5f)
            return TaskStatus.Success;

        return TaskStatus.Running;
    }

    private Vector2 GenerateTargetInRange()
    {
        int maxTries = 10;
        for (int i = 0; i < maxTries; i++)
        {
            float offsetX = Random.Range(-patrolRange, patrolRange);
            Vector2 potentialTarget = new Vector2(startPos.x + offsetX, startPos.y);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, potentialTarget, wallLayerMask);
            if (hit.collider == null)
                return potentialTarget;
        }

        return startPos;
    }
}
