using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WatchForIntruder : Conditional
{
    private Enemy enemy;

    [SerializeField] private LayerMask playerLayer;

    public SharedFloat fieldOfView = 90f;
    public SharedFloat viewDistance = 5f;
    public bool debugRays = false;

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Player.Instance == null)
            return TaskStatus.Failure;

        Vector2 origin = enemy.transform.position;
        Vector2 toPlayer = (Player.Instance.transform.position - enemy.transform.position);
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > viewDistance.Value)
            return TaskStatus.Failure;

        Vector2 forward = enemy.transform.right * Mathf.Sign(enemy.transform.localScale.x);
        float angle = Vector2.Angle(forward, toPlayer);

        if (angle <= fieldOfView.Value / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, distanceToPlayer, playerLayer);

            if (debugRays)
                Debug.DrawRay(origin, toPlayer.normalized * viewDistance.Value, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}
