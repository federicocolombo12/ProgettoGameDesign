using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    public SharedFloat patrolRange = 3f;
    public SharedFloat speed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool goingRight = true;

    public override void OnStart()
    {
        startPosition = transform.position;
        SetTarget();
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            goingRight = !goingRight;
            SetTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed.Value * Time.deltaTime);
        return TaskStatus.Running;
    }

    private void SetTarget()
    {
        Vector3 direction = goingRight ? Vector3.right : Vector3.left;
        targetPosition = startPosition + direction * patrolRange.Value;
    }
}
