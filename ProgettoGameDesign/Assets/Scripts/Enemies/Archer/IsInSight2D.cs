using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class IsInSight2D : Conditional
{
    private Enemy enemy;
    [SerializeField] private LayerMask playerLayer;
    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
        
    }

    public override TaskStatus OnUpdate()
    {
        
        
        Vector2 direction = Player.Instance.transform.position - enemy.transform.position;
        float angle = Vector2.Angle(enemy.transform.right, direction);
        
        if (angle <= 45f)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction.normalized, direction.magnitude, playerLayer);
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}
