using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WaitForIntruder : Conditional
{
    private Enemy enemy;

    [Header("Parametri visivi")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float fieldOfView = 120f;

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Player.Instance == null || enemy == null)
        {
            Debug.LogWarning(" Player.Instance o Enemy è null");
            return TaskStatus.Failure;
        }

        Vector2 toPlayer = Player.Instance.transform.position - enemy.transform.position;

        // Direzione verso cui guarda il nemico (destra o sinistra)
        Vector2 facing = enemy.transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        float angle = Vector2.Angle(facing, toPlayer);

        if (angle <= fieldOfView * 0.5f && toPlayer.magnitude <= viewDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, toPlayer.normalized, viewDistance, playerLayer);

            Debug.DrawRay(enemy.transform.position, toPlayer.normalized * viewDistance, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}
