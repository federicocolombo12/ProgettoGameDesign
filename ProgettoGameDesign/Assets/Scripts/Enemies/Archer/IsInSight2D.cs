using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class IsInSight2D : Conditional
{
    private Enemy enemy;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
        
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 direction = Player.Instance.transform.position - enemy.transform.position;
        float angle = Vector2.Angle(enemy.transform.right * enemy.transform.localScale.x, direction);

        if (angle <= 45f)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction.normalized, direction.magnitude, playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                RaycastHit2D obstacleHit = Physics2D.Raycast(enemy.transform.position, direction.normalized, direction.magnitude,wallLayer);

                if (obstacleHit.collider != null)
                {
                    return TaskStatus.Failure; // There is a wall between the enemy and the player
                }
                else
                {
                    return TaskStatus.Success; // No wall between the enemy and the player
                }
            }
        }

        return TaskStatus.Failure; // Player not in sight or angle > 45 degrees
    }

}
