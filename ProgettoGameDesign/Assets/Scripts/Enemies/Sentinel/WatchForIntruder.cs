using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WatchForIntuder : Conditional
{
    private Enemy enemy;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private float viewDistance = 5f;

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Player.Instance == null)
            return TaskStatus.Failure;

        Vector2 origin = enemy.transform.position;
        Vector2 directionToPlayer = Player.Instance.transform.position - enemy.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance)
            return TaskStatus.Failure;

        Vector2 facingDir = enemy.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(facingDir, directionToPlayer);

        if (angle <= viewAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, directionToPlayer.normalized, distanceToPlayer, playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}
