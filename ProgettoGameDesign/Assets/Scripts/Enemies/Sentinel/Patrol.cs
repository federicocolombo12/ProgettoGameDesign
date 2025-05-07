using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    private Vector2 target;
    private Rigidbody2D rb;
    private Enemy enemy;
    public SharedFloat speed;
    private Sentinel sentinel;


    public LayerMask wallLayerMask;

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
        sentinel = GetComponent<Sentinel>();
        rb = GetComponent<Rigidbody2D>();

        target = GenerateValidTarget();
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        dir.y = 0; // Ignora asse verticale per camminanti

        // Flip del nemico
        if (dir.x > 0)
        {
            enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        }
        else if (dir.x < 0)
        {
            enemy.transform.localScale = new Vector3(-Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
        }

        rb.linearVelocity = dir * speed.Value;

        if (Vector2.Distance(transform.position, target) < 0.5f)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }


    private Vector2 GenerateValidTarget()
    {
        int maxTries = 10;
        for (int i = 0; i < maxTries; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 1f;

            Vector2 potentialTarget = new Vector2(sentinel.centerOfInfluence.position.x +
                randomOffset.x, sentinel.centerOfInfluence.position.y + randomOffset.y);


            RaycastHit2D hit = Physics2D.Linecast(transform.position, potentialTarget, wallLayerMask);

            if (hit.collider == null)
            {
                return potentialTarget; 
            }
        }

        
        return (Vector2)transform.position;
    }
}
